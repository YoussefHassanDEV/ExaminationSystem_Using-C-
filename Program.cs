using System;
using System.Collections.Generic;

namespace ExaminationSystem
{
    public class Answer : ICloneable, IComparable<Answer>
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }

        public Answer(int answerId, string answerText)
        {
            AnswerId = answerId;
            AnswerText = answerText;
        }

        public object Clone()
        {
            return new Answer(AnswerId, AnswerText);
        }

        public int CompareTo(Answer? other) // Updated to handle nullable
        {
            if (other == null) return 1;
            return AnswerId.CompareTo(other.AnswerId);
        }

        public override string ToString()
        {
            return AnswerText;
        }
    }
    public abstract class Question : ICloneable, IComparable<Question>
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public double Mark { get; set; }
        public Answer[] Answers { get; set; }
        public int CorrectAnswerId { get; set; }

        protected Question(string header, string body, double mark, Answer[] answers, int correctAnswerId)
        {
            Header = header;
            Body = body;
            Mark = mark;
            Answers = answers;
            CorrectAnswerId = correctAnswerId;
        }

        public abstract void Display();

        public virtual object Clone()
        {
            Answer[] clonedAnswers = new Answer[Answers.Length];
            for (int i = 0; i < Answers.Length; i++)
            {
                clonedAnswers[i] = (Answer)Answers[i].Clone();
            }

            throw new NotImplementedException();
        }

        public int CompareTo(Question? other) // Updated to handle nullable
        {
            if (other == null) return 1;
            return Header.CompareTo(other.Header);
        }

        public override string ToString()
        {
            return Header;
        }
    }
    public class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion(string header, string body, double mark, Answer[] answers, int correctAnswerId)
            : base(header, body, mark, answers, correctAnswerId) { }

        public override void Display()
        {
            Console.WriteLine($"{Header}\n{Body}\n1. True\n2. False");
        }

        public override object Clone()
        {
            return new TrueFalseQuestion(Header, Body, Mark, (Answer[])Answers.Clone(), CorrectAnswerId);
        }
    }
    public class MCQQuestion : Question
    {
        public MCQQuestion(string header, string body, double mark, Answer[] answers, int correctAnswerId)
            : base(header, body, mark, answers, correctAnswerId) { }

        public override void Display()
        {
            Console.WriteLine($"{Header}\n{Body}");
            for (int i = 0; i < Answers.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Answers[i].AnswerText}");
            }
        }

        public override object Clone()
        {
            return new MCQQuestion(Header, Body, Mark, (Answer[])Answers.Clone(), CorrectAnswerId);
        }
    }
    public abstract class Exam : ICloneable, IComparable<Exam>
    {
        public TimeSpan Time { get; set; }
        public int NumOfQuestions { get; set; }
        public Subject Subject { get; set; }
        public Question[] Questions { get; set; }

        protected Exam(TimeSpan time, int numOfQuestions, Subject subject, Question[] questions)
        {
            Time = time;
            NumOfQuestions = numOfQuestions;
            Subject = subject;
            Questions = questions;
        }

        public abstract void ShowExam();

        public abstract double Evaluate(int[] studentAnswers);

        public object Clone()
        {
            Exam clone = (Exam)MemberwiseClone();
            clone.Subject = (Subject)Subject.Clone();
            clone.Questions = (Question[])Questions.Clone();
            return clone;
        }

        public int CompareTo(Exam? other) // Updated to handle nullable
        {
            if (other == null) return 1;
            return Subject.CompareTo(other.Subject);
        }

        public override string ToString()
        {
            return Subject.SubjectName;
        }
    }
    public class FinalExam : Exam
    {
        public FinalExam(TimeSpan time, int numOfQuestions, Subject subject, Question[] questions)
            : base(time, numOfQuestions, subject, questions) { }

        public override void ShowExam()
        {
            Console.WriteLine($"Final Exam for {Subject.SubjectName}");
            Console.WriteLine($"Duration: {Time}\nNumber of Questions: {NumOfQuestions}");
            foreach (var question in Questions)
            {
                question.Display();
            }
        }

        public override double Evaluate(int[] studentAnswers)
        {
            double totalMark = 0.0;
            for (int i = 0; i < studentAnswers.Length; i++)
            {
                if (studentAnswers[i] == Questions[i].CorrectAnswerId)
                {
                    totalMark += Questions[i].Mark;
                }
            }
            return totalMark;
        }
    }
    public class PracticalExam : Exam
    {
        public PracticalExam(TimeSpan time, int numOfQuestions, Subject subject, Question[] questions)
            : base(time, numOfQuestions, subject, questions) { }

        public override void ShowExam()
        {
            Console.WriteLine($"Practical Exam for {Subject.SubjectName}");
            Console.WriteLine($"Duration: {Time}\nNumber of Questions: {NumOfQuestions}");
            foreach (var question in Questions)
            {
                question.Display();
            }
        }

        public override double Evaluate(int[] studentAnswers)
        {
            double totalMark = 0.0;
            for (int i = 0; i < studentAnswers.Length; i++)
            {
                if (studentAnswers[i] == Questions[i].CorrectAnswerId)
                {
                    totalMark += Questions[i].Mark;
                    Console.WriteLine($"Your answer is correct: {Questions[i].Answers[studentAnswers[i] - 1].AnswerText}");
                }
                else
                {
                    Console.WriteLine($"Your answer is wrong. The correct answer is: {Questions[i].Answers[Questions[i].CorrectAnswerId - 1].AnswerText}");
                }
            }
            return totalMark;
        }
    }

    public class Subject : ICloneable, IComparable<Subject>
    {
        public int SubjectId { get; set; ICloneable}
        public string SubjectName { get; set; }
        public List<Exam> Exams { get; set; }

        public Subject(int subjectId, string subjectName)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
            Exams = new List<Exam>();
        }

        public void AddExam(Exam exam)
        {
            Exams.Add(exam);
        }

        public object Clone()
        {
            return new Subject(SubjectId, SubjectName);
        }

        public int CompareTo(Subject? other) // Updated to handle nullable
        {
            if (other == null) return 1;
            return SubjectId.CompareTo(other.SubjectId);
        }

        public override string ToString()
        {
            return SubjectName;
        }
    }

    public abstract class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        protected User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public abstract void DisplayMenu();
    }

    public class Teacher : User
    {
        public int TeacherId { get; set; }

        public Teacher(string userName, string password, int teacherId)
            : base(userName, password)
        {
            TeacherId = teacherId;
        }

        public override void DisplayMenu()
        {
            Console.WriteLine("1. Create Exam");
            Console.WriteLine("2. Show Student Grades");
            Console.WriteLine("3. Logout");
        }

        public void CreateExam(List<Subject> subjects)
        {
            Console.Write("Enter Subject ID: ");
            int subjectId = int.Parse(Console.ReadLine());

            Subject subject = subjects.Find(s => s.SubjectId == subjectId);

            if (subject == null)
            {
                Console.WriteLine("Subject not found.");
                return;
            }

            Console.Write("Enter Exam Duration (hours): ");
            double hours = double.Parse(Console.ReadLine());
            TimeSpan duration = TimeSpan.FromHours(hours);

            Console.Write("Enter Number of Questions: ");
            int numQuestions = int.Parse(Console.ReadLine());

            Question[] questions = new Question[numQuestions];

            for (int i = 0; i < numQuestions; i++)
            {
                Console.Write("Enter Question Type (1. True/False, 2. MCQ): ");
                int questionType = int.Parse(Console.ReadLine());

                Console.Write("Enter Question Header: ");
                string header = Console.ReadLine();

                Console.Write("Enter Question Body: ");
                string body = Console.ReadLine();

                Console.Write("Enter Question Mark: ");
                double mark = double.Parse(Console.ReadLine());

                if (questionType == 1)
                {
                    Answer[] answers = new Answer[]
                    {
                        new Answer(1, "True"),
                        new Answer(2, "False")
                    };

                    Console.Write("Enter Correct Answer ID (1. True, 2. False): ");
                    int correctAnswerId = int.Parse(Console.ReadLine());

                    questions[i] = new TrueFalseQuestion(header, body, mark, answers, correctAnswerId);
                }
                else if (questionType == 2)
                {
                    Console.Write("Enter Number of Choices: ");
                    int numChoices = int.Parse(Console.ReadLine());

                    Answer[] answers = new Answer[numChoices];

                    for (int j = 0; j < numChoices; j++)
                    {
                        Console.Write($"Enter Choice {j + 1} Text: ");
                        string choiceText = Console.ReadLine();
                        answers[j] = new Answer(j + 1, choiceText);
                    }

                    Console.Write("Enter Correct Answer ID: ");
                    int correctAnswerId = int.Parse(Console.ReadLine());

                    questions[i] = new MCQQuestion(header, body, mark, answers, correctAnswerId);
                }
                else
                {
                    Console.WriteLine("Invalid question type.");
                    i--; // Repeat this iteration
                }
            }

            Console.Write("Enter Exam Type (1. Final, 2. Practical): ");
            int examType = int.Parse(Console.ReadLine());

            if (examType == 1)
            {
                FinalExam finalExam = new FinalExam(duration, numQuestions, subject, questions);
                subject.AddExam(finalExam);
            }
            else if (examType == 2)
            {
                PracticalExam practicalExam = new PracticalExam(duration, numQuestions, subject, questions);
                subject.AddExam(practicalExam);
            }
            else
            {
                Console.WriteLine("Invalid exam type.");
            }
        }

        public void ShowStudentGrades(List<Student> students)
        {
            foreach (var student in students)
            {
                Console.WriteLine($"Student: {student.UserName}");
                foreach (var examResult in student.ExamResults)
                {
                    Console.WriteLine($"{examResult.Key.Subject.SubjectName}: {examResult.Value}");
                }
            }
        }
    }

    public class Student : User
    {
        public int StudentId { get; set; }
        public Dictionary<Exam, double> ExamResults { get; set; }

        public Student(string userName, string password, int studentId)
            : base(userName, password)
        {
            StudentId = studentId;
            ExamResults = new Dictionary<Exam, double>();
        }

        public override void DisplayMenu()
        {
            Console.WriteLine("1. Take Exam");
            Console.WriteLine("2. Show Grades");
            Console.WriteLine("3. Logout");
        }

        public void TakeExam(List<Subject> subjects)
        {
            Console.Write("Enter Subject ID: ");
            int subjectId = int.Parse(Console.ReadLine());

            Subject subject = subjects.Find(s => s.SubjectId == subjectId);

            if (subject == null)
            {
                Console.WriteLine("Subject not found.");
                return;
            }

            Console.WriteLine("Available Exams:");
            for (int i = 0; i < subject.Exams.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {subject.Exams[i].GetType().Name} ({subject.Exams[i].Time})");
            }

            Console.Write("Enter Exam Number: ");
            int examNumber = int.Parse(Console.ReadLine());

            if (examNumber < 1 || examNumber > subject.Exams.Count)
            {
                Console.WriteLine("Invalid exam number.");
                return;
            }

            Exam selectedExam = subject.Exams[examNumber - 1];
            selectedExam.ShowExam();

            int[] studentAnswers = new int[selectedExam.Questions.Length];

            for (int i = 0; i < selectedExam.Questions.Length; i++)
            {
                Console.Write($"Enter your answer for Question {i + 1}: ");
                studentAnswers[i] = int.Parse(Console.ReadLine());
            }

            double score = selectedExam.Evaluate(studentAnswers);
            ExamResults[selectedExam] = score;

            Console.WriteLine($"Your score: {score}");
        }

        public void ShowGrades()
        {
            foreach (var examResult in ExamResults)
            {
                Console.WriteLine($"{examResult.Key.Subject.SubjectName}: {examResult.Value}");
            }
        }
    }

    public class Program
    {
        private static List<User> users = new List<User>();
        private static List<Subject> subjects = new List<Subject>();

        public static void Main(string[] args)
        {
            InitializeData();

            while (true)
            {
                Console.WriteLine("Are you a (1) Teacher or (2) Student?");
                string userType = Console.ReadLine();

                if (userType == "1" || userType == "2")
                {
                    Console.Write("Enter Username: ");
                    string username = Console.ReadLine();

                    Console.Write("Enter Password: ");
                    string password = Console.ReadLine();

                    User user = AuthenticateUser(username, password, userType);

                    if (user != null)
                    {
                        Console.WriteLine($"Welcome, {user.UserName}!");

                        bool loggedIn = true;
                        while (loggedIn)
                        {
                            user.DisplayMenu();
                            string choice = Console.ReadLine();

                            if (user is Teacher teacher)
                            {
                                switch (choice)
                                {
                                    case "1":
                                        teacher.CreateExam(subjects);
                                        break;
                                    case "2":
                                        teacher.ShowStudentGrades(users.FindAll(u => u is Student).ConvertAll(u => (Student)u));
                                        break;
                                    case "3":
                                        loggedIn = false;
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice. Please try again.");
                                        break;
                                }
                            }
                            else if (user is Student student)
                            {
                                switch (choice)
                                {
                                    case "1":
                                        student.TakeExam(subjects);
                                        break;
                                    case "2":
                                        student.ShowGrades();
                                        break;
                                    case "3":
                                        loggedIn = false;
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice. Please try again.");
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 1 for Teacher or 2 for Student.");
                }
            }
        }

        private static void InitializeData()
        {
            // Initialize subjects
            subjects.Add(new Subject(1, "Math"));
            subjects.Add(new Subject(2, "Science"));

            // Initialize users
            users.Add(new Teacher("teacher1", "pass1", 1));
            users.Add(new Student("student1", "pass1", 1));
        }

        private static User AuthenticateUser(string username, string password, string userType)
        {
            foreach (User user in users)
            {
                if (user.UserName == username && user.Password == password)
                {
                    if ((userType == "1" && user is Teacher) || (userType == "2" && user is Student))
                    {
                        return user;
                    }
                }
            }
            return null;
        }
    }
}
