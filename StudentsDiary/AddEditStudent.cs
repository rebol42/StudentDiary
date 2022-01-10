using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        // private string _filePath = Path.Combine(Environment.CurrentDirectory, "StudentsDiary.txt");
        /* Obłsuga evetnów przez delegaty Tyd5 lekcja19
        public delegate void MySimpleDelegate();
        public event MySimpleDelegate StudentAdded;
        */
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        private int _studentId;

        private Student _student;


        public AddEditStudent(int id = 0)
        {
            InitializeComponent();

            _studentId = id;


            GetStudentData();
            tbFirstName.Select();

            
        }
        /* Obłsuga evetnów przez delegaty Tyd5 lekcja19
        private void OnStudentAdded()
        {
            StudentAdded?.Invoke();
        }
        */
        private void GetStudentData()
        {
            if (_studentId != 0)
            {

                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym id");

                FillTextBoxes();

            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            cbIdGroup.Text = _student.IdGroup;
            rtbComments.Text = _student.Comments;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPhiz.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            chkAddClasses.Checked = _student.additionalClasses;
           
        }

       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {

            var students = _fileHelper.DeserializeFromFile();



            if(_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);

            /*
             var studentId = 0;
            if (studentMaxid == null)
            {
                studentId = 1;
            }
            else
                studentId = studentMaxid.Id + 1;
            */

            AddNewUserToList(students);
            _fileHelper.SerializeToFile2(students);

            //OnStudentAdded(); Obłsuga evetnów przez delegaty Tyd5 lekcja19
            await LongProcessAsync();

            Close();
        
        }
        /* ten zapis jest zwiazany z delegatem action w metodzie LongProcessAsync
        public void ActionDelegateMethod()
        {
            Thread.Sleep(3000);
        }
        */
        private async Task LongProcessAsync()
        {
            /* 
             * to wykonuje to samo to kod poniżej
            var action = new Action(ActionDelegateMethod);
            Task.Run(action);
            */

            await Task.Run(() =>
            {

                Thread.Sleep(3000);
            });

        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentMaxid = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentMaxid == null ? 1 : studentMaxid.Id + 1;
        }

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                IdGroup = cbIdGroup.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhiz.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                additionalClasses = chkAddClasses.Checked
               

            };

            students.Add(student);
        }

      
    }
}
