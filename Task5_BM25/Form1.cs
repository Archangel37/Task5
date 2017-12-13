using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Task5_BM25
{
    public partial class Form1 : Form
    {
        //(*) https://ru.wikipedia.org/wiki/Okapi_BM25 
        //https://habrahabr.ru/post/81592/ - на почитать

        private const double k1 = 2, b = 0.75;
        private static string Dict = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ";
        private string[] filesTxt;
        private List<TextDefinition> OurLibrary = new List<TextDefinition>();
        private readonly string Separator = "_____________________________" + Environment.NewLine;
        private readonly string Separator2 = "............................." + Environment.NewLine;

        public Form1()
        {
            InitializeComponent();
            Evaluate.Enabled = false;
            textBox_search_string.Enabled = false;
            checkBox_TxtOrUrl.Checked = true;
        }

        //убрать лишние табуляции, переводы каретки и новую строку, пробелы вначале и конце
        private string NRT_Remove(string input) => input.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Trim();

        //убираем всё, кроме Кириллицы и пробелов (!!!!) - входная строка должна быть мелкими буквами,
        //иначе придётся расширять значения Dict
        private string Normalizer(string input) => 
        input.Where(ch => Dict.Contains(ch)).Aggregate(string.Empty, (cur, ch) => cur + ch);

        //тут получается итоговый набор слов мелкими буквами, кириллица с одним пробелом между словами и в одну строку
        private string[] SpacesAndUpperRemove(string input) => Normalizer(Regex.Replace(NRT_Remove(input), "[ ]+", " ").ToLower()).Split(' ');

        //public static int WordsCounter(string input) => input.Split(' ').Length;


        private double nq(string q, List<TextDefinition> listTexts) => listTexts.Count(x => x.NormalizedTextFromSource.Contains(q));

        //посчитать IDF - см описание (*)
        //взять основание для непрерывных систем е или 33 для буквенных - дискретных - пока что взято е, но с серьёзной работой 
        //- следует брать по количеству символов алфавита - если Англ+Ру = 33+26 и т.д. (теория информационных систем, дискретная математика) 
        //деление на 0 в простой формуле, если nq==0, взял модифицированную
        private double IDF(string q, List<TextDefinition> listTexts) => Math.Log(
                (listTexts.Count - nq(q, listTexts) + 0.5) /
                (nq(q, listTexts) + 0.5));


        //среднее число слов - avgdl - см описание (*)
        private double avgdl(List<TextDefinition> listTexts) => (double) listTexts.Aggregate(0, (cur, x) => cur + x.NumWordsInSource) / listTexts.Count;

        //частота слова в документе
        private double FrequencyWord(string q, TextDefinition fileTxt) => (double) fileTxt.NormalizedTextFromSource.Count(word => word == q) / fileTxt.NumWordsInSource;


        //баллы одного слова в одном тексте
        private double Score(string q, List<TextDefinition> listTexts, TextDefinition D)
        => IDF(q, listTexts) * FrequencyWord(q, D) * (k1 + 1) /
                   (FrequencyWord(q, D) + k1 * (1 - b + b * D.NumWordsInSource / avgdl(listTexts)));


        //баллы всего запроса в одном файле
        //принято решение игнорировать отрицательные(!!!) - часто входящие слова в более чем половину документов 
        private double ScoreManyWords(string[]
            request, List<TextDefinition> listTexts, TextDefinition D)
        => request.Sum(q => Score(q, listTexts, D) >= 0 ? Score(q, listTexts, D) : 0);
        


        private void Evaluate_Click(object sender, EventArgs e)
        {
            richTextBox_result.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(textBox_search_string.Text))
            {
                MessageBox.Show("Please, enter search words", "Empty search options", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            //можно вводить любую кириллицу и знаки, знаки игнорятся - к примеру, "Бесы, котОрые всполошились"
            var searchOptions = Regex.Replace(Normalizer(textBox_search_string.Text.ToLower()), "[ ]+", " ").Split(' ');

            foreach (var element in OurLibrary)
                element.ScoreText = ScoreManyWords(searchOptions, OurLibrary, element);

            OurLibrary.Sort();
            OurLibrary.Reverse();

            foreach (var element in OurLibrary)
            {
                if (OurLibrary.Count(x=> x.ScoreText>0) >0 && element.ScoreText>0)
                richTextBox_result.Text +=
                    (element == OurLibrary[0] && element.ScoreText > 0 ? "Winner! -> " : string.Empty) + "Score: " +
                    element.ScoreText + " Source: " + element.Path + Environment.NewLine;
                else if (OurLibrary.Count(x => x.ScoreText > 0) == 0)
                {
                    richTextBox_result.Text += "No matches found.. Try to change Your request" + Environment.NewLine;
                    break;
                }
            }
            // можно вывести детали источников - сколько слов в каждом
            //richTextBox_result.Text += Separator + "Details:" + Environment.NewLine;
            //foreach (var element in OurLibrary)
            //    richTextBox_result.Text +=
            //        element.Path + "  -> " + element.NumWordsInSource + " words" + Environment.NewLine;

            //среднее число слов в источниках
            richTextBox_result.Text += "AVG words: " + avgdl(OurLibrary) + Environment.NewLine;
            
            //подробности значений поиска наглядно
            richTextBox_result.Text += Separator + "Search details:" + Environment.NewLine;
            foreach (var qSearchOption in searchOptions)
            {
                richTextBox_result.Text += qSearchOption + " -> IDF: " + IDF(qSearchOption, OurLibrary) +
                                           (IDF(qSearchOption, OurLibrary) < 0 ? " (thick word, ignore)" : "") +
                                           Environment.NewLine;
                richTextBox_result.Text +=
                    qSearchOption + " -> n(qi): " + nq(qSearchOption, OurLibrary) + Environment.NewLine + Separator2;
               
            }
        }


        private void FromTextFiles()
        {
            var DateStart = DateTime.Now;
            //OurLibrary.Clear();
            OurLibrary = new List<TextDefinition>();
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog(this) != DialogResult.OK) return;

                textBox_path.Text = folderDialog.SelectedPath;
                filesTxt = Directory.GetFiles(folderDialog.SelectedPath, "*.txt");

                //стараемся параллельно считывать значения
                System.Threading.Tasks.Parallel.ForEach(filesTxt, file =>
                    //foreach (var file in filesTxt)
                {
                    OurLibrary.Add(new TextDefinition());
                    OurLibrary[OurLibrary.Count - 1].Path = file;
                    
                    //продумать использование MMF
                    //using (var mmf =
                    //    MemoryMappedFile.CreateFromFile(file, FileMode.Open, file))
                    //using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(0, new FileInfo(file).Length))
                    //{

                    //    //OurLibrary[OurLibrary.Count - 1].NormalizedTextFromFile = 
                    //}
                    //(!!!)Файлы только в UTF-8, по поводу кодировок не стал пока заморачиваться
                    //продумать ускорение - на "больших" файлах просто ппц (1мб+)
                    OurLibrary[OurLibrary.Count - 1].NormalizedTextFromSource =
                        SpacesAndUpperRemove(File.ReadAllText(file));
                });
               
            }
            //для последующего сравнения для оптимизации вывожу себе время
            richTextBox_result.Text += DateTime.Now - DateStart;
        }

        


        private void FromXmlUrl()
        {
            //OurLibrary.Clear();
            OurLibrary = new List<TextDefinition>();
            textBox_path.Text = "http://bash.im/rss/";

            XDocument doc;
            using (var xmlReader = new XmlTextReader("http://bash.im/rss/"))
                doc = XDocument.Load(xmlReader);
            
            
            //!!!!
            foreach (XElement el in doc.Root.Elements())
            {
                foreach (XElement element in el.Elements())
                foreach (var littlElement in element.Elements())
                    {
                        if (littlElement.Name == "link")
                        {
                            OurLibrary.Add(new TextDefinition());
                            //на случай пустого пути
                            OurLibrary[OurLibrary.Count-1].Path = littlElement.Value.Length >0 ? littlElement.Value : "http:";                           
                        }
                        if (littlElement.Name == "description")
                            //на случай написания символами или латиницей всего дескрипшена
                            OurLibrary[OurLibrary.Count - 1].NormalizedTextFromSource =
                                SpacesAndUpperRemove(littlElement.Value).Length >0 ? SpacesAndUpperRemove(littlElement.Value) : SpacesAndUpperRemove("ААА ббб"); 
                    }
            }
        }


        private void button_source_Click(object sender, EventArgs e)
        {
            if (checkBox_TxtOrUrl.Checked) FromXmlUrl();
            else FromTextFiles();

            Evaluate.Enabled = true;
            textBox_search_string.Enabled = true;
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! класс текстовиков
        private class TextDefinition : IComparable<TextDefinition>
        {
            public string Path { get; set; }

            public string[] NormalizedTextFromSource { get; set; }

            public int NumWordsInSource => NormalizedTextFromSource.Length;
            public double ScoreText { get; set; }

            //компаратор - для работы сортировки
            public int CompareTo(TextDefinition p) => ScoreText.CompareTo(p.ScoreText);
            
        }

    }
}