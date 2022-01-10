using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public class FileHelper<T> where T : new() // <T> - typ generyczny, możemy wtedy w metodach Serialize i Deserialize                              obsługiwać 
                                // nie tylko klasę student , tylko rózne klasy i dlatego zamieniamy List<Student> na <T>
    {
        private string _filePath;


        public FileHelper(string filePath)
        {
            _filePath = filePath;
        }

        public void SerializeToFile(T students)
        {
            var serializer = new XmlSerializer(typeof(T));
            var streamWriter = new StreamWriter(_filePath);

            try
            {

                serializer.Serialize(streamWriter, students);
                //klasa streamWriter zapewnia nam transfer bajtów (odczytywanie i zapisywanie do źródła)
                streamWriter.Close();
                streamWriter.Dispose();
            }
            finally
            {
                streamWriter.Dispose();
            }


        }

        public void SerializeToFile2(T students)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var streamWriter = new StreamWriter(_filePath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }

        }


        public T DeserializeFromFile()
        {

            if (!File.Exists(_filePath))
            {
                return new T();
            }

            var serializer = new XmlSerializer(typeof(T));

            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (T)serializer.Deserialize(streamReader);
                streamReader.Close();

                return students;
            }


        }
    }
}
