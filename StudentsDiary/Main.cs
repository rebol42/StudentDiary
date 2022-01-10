using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace StudentsDiary
{
    public partial class Main : Form
    {
       // private string _filePath = Path.Combine(Environment.CurrentDirectory, "StudentsDiary.txt");

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        public bool isMazimize
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }

        }

        public Main()
        {
           
            InitializeComponent();
            RefreshDiary();
            SetColumnsHeader();
            PopulateCbIdGroup();

            if (isMazimize)
                WindowState = FormWindowState.Maximized;

        /*  Lekcja 13 tydzien 5 -> LINQ
            var list = new List<int> { 2, 42, 22, 5, 85 };

            var list2 = (from x in list
                            where x > 10
                            orderby x
                            select x).ToList();


            var list3 = list.Where(x => x > 10).OrderBy(x => x).ToList();

            var students = new List<Student>();

          / var student1 = from x in students
                           select x.Id;

            var student2 = students.Select(x => x.Id);

            var allPositives = list.All(x => x > 0);

            MessageBox.Show(allPositives.ToString());

            var anyNumberBiggerThan100 = list.Any(x => x > 100);

            MessageBox.Show(anyNumberBiggerThan100.ToString());

            var contain10 = list.Contains(10);

            var sum = list.Sum();
            var count = list.Count();
            var avg = list.Average();
            var max = list.Max();
            var firstElement = list.First(x => x > 10);
            */

            /* ############ Wyremowanie z lekcji 8 ###############
           var filePath = $@"{System.IO.Path.GetDirectoryName(Application.ExecutablePath)}\..\Data.txt";

            nie trzeba uzywać takiego zapisu jak używasz AppendAllText (ten warunek jest zaszyty w tej metodzie
           if (!File.Exists(filePath))
           {
               File.Create(filePath);
           }


           // File.Delete(filePath);
         //  File.WriteAllText(filePath, "Zostań");
           File.AppendAllText(filePath, "Dodaj nowy tekst\n");

           var text = File.ReadAllText(filePath);
           MessageBox.Show(text);
           MessageBox.Show("Test", "Tytul", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
           */


            /*########### tutaj remujemy to co ponizej z Lekcji 9 ################
            var students = new List<Student>();
            
            students.Add(new Student { FirstName = "Przemek" });
            students.Add(new Student { FirstName = "Piotr" });
            students.Add(new Student { FirstName = "Paweł" });

            SerializeToFile2(students);
            

            students = DeserializeFromFile();

            foreach(var student in students)
            {
                MessageBox.Show(student.FirstName);
            }
            */
        }

        private void SetColumnsHeader()
        {
            dgfDiary.Columns[0].HeaderText = "Numer";
            dgfDiary.Columns[1].HeaderText = "Imię";
            dgfDiary.Columns[2].HeaderText = "Nazwisko";
            dgfDiary.Columns[3].HeaderText = "Id Grupy";
            dgfDiary.Columns[4].HeaderText = "Uwagi";
            dgfDiary.Columns[5].HeaderText = "Matematyka";
            dgfDiary.Columns[6].HeaderText = "Technologia";
            dgfDiary.Columns[7].HeaderText = "Fizyka";
            dgfDiary.Columns[8].HeaderText = "Język Polski";
            dgfDiary.Columns[9].HeaderText = "Język Obcy";
            dgfDiary.Columns[10].HeaderText = "Dodatkowe zajęcia";

        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            dgfDiary.DataSource = students;
        }
        
        // Metoda która uzupełnia ComboBoxa o wartości idGrup już wybranych przy dodawaniu studentów.
        private void PopulateCbIdGroup()
        {
            var students = _fileHelper.DeserializeFromFile();
            int idGroup = 0;

            Dictionary<string, string> getFromStudents = new Dictionary<string, string>();

            getFromStudents.Add(idGroup.ToString(), "Wszyscy");

            foreach (var student in students)
            {
                idGroup++;

                if(!getFromStudents.ContainsValue(student.IdGroup))
                    getFromStudents.Add(idGroup.ToString(), student.IdGroup);

            }
           
          
            cbSelectGroup.DataSource = new BindingSource(getFromStudents, null);
            cbSelectGroup.DisplayMember = "Value";
            cbSelectGroup.ValueMember = "Key";

            
        }
        // metoda filtrowania datasource na podstawie wartości w ComoboBox.
        private void SelectStudentByIDGroup(string groupId)
        {


            if (groupId == "Wszyscy")
                RefreshDiary();
            else
            {
                
                var students = _fileHelper.DeserializeFromFile();

                var student2 =  from studentsQ in students
                                where studentsQ.IdGroup == groupId 
                                select studentsQ;

                dgfDiary.DataSource = student2.ToList();
                
              
            }

           // MessageBox(student2);
        }

      

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditStudent addStudent = new AddEditStudent();

            addStudent.FormClosing += AddStudent_FormClosing;
           // addStudent.StudentAdded += AddStudent_StudentAdded;  obłsuga swoich zdarzeń przez delegaty Tyd5 lekcja19
            addStudent.ShowDialog();
            //addStudent.StudentAdded -= AddStudent_StudentAdded; obłsuga swoich zdarzeń przez delegaty Tyd5 lekcja19
        }

        private void AddStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        /* Obłśuga zdarzeń przez delegaty Tyd5 lekcja19
private void AddStudent_StudentAdded()
{
   RefreshDiary();
}
*/
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(dgfDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia do edycji");
                return;
            }
            else
            {
                AddEditStudent addStudent = new AddEditStudent(
                    Convert.ToInt32(dgfDiary.SelectedRows[0].Cells[0].Value));
                addStudent.FormClosing += AddStudent_FormClosing;
                addStudent.ShowDialog();

                
            }
               
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgfDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego chcesz usunąć");
                return;
            }
            else
            {
                var selectedStudent = dgfDiary.SelectedRows[0];

                var confirmDelete = 
                MessageBox.Show($"Czy na pewno chcesz usunać ucznia { (selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()}", "Usuwanie ucznia",MessageBoxButtons.OKCancel);


                if(confirmDelete == DialogResult.OK)
                {
                    DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                    RefreshDiary();

                }
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();

            students.RemoveAll(x => x.Id == id);

            _fileHelper.SerializeToFile2(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                isMazimize = true;
            else
                isMazimize = false;

            Settings.Default.Save();
        }

        private void cbSelectGroup_SelectionChanged(object sender, EventArgs e)
        {
            string value = ((KeyValuePair<string, string>)cbSelectGroup.SelectedItem).Value;


                SelectStudentByIDGroup(value);
        }
    }
}
