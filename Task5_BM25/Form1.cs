using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Task5_BM25
{
    public partial class Form1 : Form
    {
        //(*) https://ru.wikipedia.org/wiki/Okapi_BM25 
        //https://habrahabr.ru/post/81592/

        private const double k1 = 2, b = 0.75;
        private static string Dict = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ";
        private string[] filesTxt;
        private List<TextDefinition> OurLibrary = new List<TextDefinition>();
        private readonly string Separator = "_____________________________" + Environment.NewLine;

        public Form1()
        {
            InitializeComponent();
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


        private double nq(string q, List<TextDefinition> listTexts) => listTexts.Count(x => x.NormalizedTextFromFile.Contains(q));

        //посчитать IDF - см описание (*)
        //взять основание для непрерывных систем е или 33 для буквенных - дискретных - пока что взято е, но с серьёзной работой 
        //- следует брать по количеству символов алфавита - если Англ+Ру = 33+26 и т.д. (теория информационных систем, дискретная математика) 
        //деление на 0 в простой формуле, если nq==0, взял модифицированную
        private double IDF(string q, List<TextDefinition> listTexts) => Math.Log(
                (listTexts.Count - nq(q, listTexts) + 0.5) /
                (nq(q, listTexts) + 0.5));


        //среднее число слов - avgdl - см описание (*)
        private double avgdl(List<TextDefinition> listTexts) => (double) listTexts.Aggregate(0, (cur, x) => cur + x.NumWordsInFile) / listTexts.Count;

        //частота слова в документе
        private double FrequencyWord(string q, TextDefinition fileTxt) => (double) fileTxt.NormalizedTextFromFile.Count(word => word == q) / fileTxt.NumWordsInFile;


        //баллы одного слова в одном тексте
        private double Score(string q, List<TextDefinition> listTexts, TextDefinition D)
        => IDF(q, listTexts) * FrequencyWord(q, D) * (k1 + 1) /
                   (FrequencyWord(q, D) + k1 * (1 - b + b * D.NumWordsInFile / avgdl(listTexts)));


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
                richTextBox_result.Text += (element == OurLibrary[0] ? "Winner! -> " : string.Empty) + "Score: " +
                                           element.ScoreText +
                                           " Book: " + element.Path + Environment.NewLine;

            //
            richTextBox_result.Text += Separator + "Details:" + Environment.NewLine;
            foreach (var element in OurLibrary)
                richTextBox_result.Text +=
                    element.Path + "  -> " + element.NumWordsInFile + " words" + Environment.NewLine;
            //
            richTextBox_result.Text += "AVG words: " + avgdl(OurLibrary) + Environment.NewLine;
            richTextBox_result.Text += Separator + "Search details:" + Environment.NewLine;
            foreach (var qSearchOption in searchOptions)
            {
                richTextBox_result.Text += qSearchOption + " -> IDF: " + IDF(qSearchOption, OurLibrary) +
                                           (IDF(qSearchOption, OurLibrary) < 0 ? " (thick word, ignore)" : "") +
                                           Environment.NewLine;
                richTextBox_result.Text +=
                    qSearchOption + " -> n(qi): " + nq(qSearchOption, OurLibrary) + Environment.NewLine;
            }
        }


        private void button_source_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog(this) != DialogResult.OK) return;

                textBox_path.Text = folderDialog.SelectedPath;
                filesTxt = Directory.GetFiles(folderDialog.SelectedPath, "*.txt");

                foreach (var file in filesTxt)
                {
                    OurLibrary.Add(new TextDefinition());
                    OurLibrary[OurLibrary.Count - 1].Path = file;
                    //(!!!)Файлы только в UTF-8, по поводу кодировок не стал пока заморачиваться
                    //продумать ускорение - на больших файлах просто ппц
                    OurLibrary[OurLibrary.Count - 1].NormalizedTextFromFile =
                        SpacesAndUpperRemove(File.ReadAllText(file));
                }
            }
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! класс текстовиков
        private class TextDefinition : IComparable<TextDefinition>
        {
            public string Path { get; set; }

            public string[]
                NormalizedTextFromFile
            {
                get;
                set;
            } // => SpacesAndUpperRemove(File.ReadAllText(Path)); - требуется статичный метод, но тогда текста везде будут одинаковые

            public int NumWordsInFile => NormalizedTextFromFile.Length;
            public double ScoreText { get; set; }

            public int CompareTo(TextDefinition p)
            {
                return ScoreText.CompareTo(p.ScoreText);
            }
        }
    }
}