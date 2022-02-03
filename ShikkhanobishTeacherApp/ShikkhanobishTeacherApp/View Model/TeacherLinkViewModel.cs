using Flurl.Http;
using ShikkhanobishTeacherApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ShikkhanobishTeacherApp.View_Model
{
    public class TeacherLinkViewModel:BaseViewMode, INotifyPropertyChanged
    {

        List<Teacher> teacherList = new List<Teacher>();
        List<Chapter> chapList = new List<Chapter>();
        List<Topic> tList = new List<Topic>();
        List<TuiTionLog> tuiTionList = new List<TuiTionLog>();
        List<Subject> subList = new List<Subject>();
        List<ClassInfo> classList = new List<ClassInfo>();

        public TeacherLinkViewModel()
        {
            string tuioionid = "10000";
            GetTutionDetails();
            GetTopic(tuioionid);
            
        }
        #region Methods
        public async Task GetTutionDetails()
        {
            teacherList = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getAllTeacher".PostJsonAsync(new { }).ReceiveJson<List<Teacher>>();
            tuiTionList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".PostJsonAsync(new { }).ReceiveJson<List<TuiTionLog>>();
            subList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getSubject".GetJsonAsync<List<Subject>>();
            classList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getClassInfo".GetJsonAsync<List<ClassInfo>>();


            

            foreach (var teacher in teacherList)
            {
                if (StaticPageForPassingData.thisTeacher.teacherID == teacher.teacherID)
                {
                    teacherName = teacher.name;
                }
            }

            for (int i=0; i < tuiTionList.Count; i++)
            {
                if(tuiTionList[i].studentID== 10000152)
                {
                    for(int j=0; j < subList.Count; j++)
                    {
                        if (tuiTionList[i].subjectID==subList[j].subjectID)
                        {
                            subName = subList[j].name;

                            string sub = "";
                            for (int m=0; m<classList.Count; m++) {
                                if (subList[j].classID == classList[m].classID)
                                {
                                    className = classList[m].name;
                                   
                                    sub = subList[j].name;   
                                }
                            }
                            subjectList = sub + ", ";
                        }
                    }
                }
            }
            
        }
        public async Task GetTopic(string tuioionid)
        {
            chapList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getChapter".GetJsonAsync<List<Chapter>>();
            tList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTopic".GetJsonAsync<List<Topic>>();
            var tuiTionList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".PostJsonAsync(new { }).ReceiveJson<List<TuiTionLog>>();

            foreach (var tuition in tuiTionList)
            {
                if (tuition.tuitionLogID == tuioionid)  
                {
                    studentName = tuition.studentName;
                    foreach (var chap in chapList)
                    {
                        if (tuition.chapterID == chap.chapterID)
                        {
                            chapName = chap.name;
                            List<Topic> tp = new List<Topic>();
                            foreach (var topic in tList)
                            {
                                if (topic.chapterID == chap.chapterID)
                                {
                                    tp.Add(topic);
                                }
                               
                            }
                            topicList = tp;
                        }
                    }
                }
                
            }

            

        }
        #endregion

        #region Bindings
        private string subjectList1;
        public string subjectList { get => subjectList1; set => SetProperty(ref subjectList1, value); }

        private string teacherName1;
        public string teacherName { get => teacherName1; set => SetProperty(ref teacherName1, value); }

        private string studentName1;
        public string studentName { get => studentName1; set => SetProperty(ref studentName1, value); }

        private string subName1;
        public string subName { get => subName1; set => SetProperty(ref subName1, value); }

        private string className1;
        public string className { get => className1; set => SetProperty(ref className1, value); }

        private string chapName1;
        public string chapName { get => chapName1; set => SetProperty(ref chapName1, value); }

        private List<Topic> topicList1;

        public List<Topic> topicList { get => topicList1; set => SetProperty(ref topicList1, value); }

        #endregion

    }
}
