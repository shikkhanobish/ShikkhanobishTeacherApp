using Flurl.Http;
using Microsoft.AspNetCore.SignalR.Client;
using ShikkhanobishTeacherApp.Model;
using ShikkhanobishTeacherApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace ShikkhanobishTeacherApp.View_Model
{
    public class TuitionLogViewModel : BaseViewMode, INotifyPropertyChanged
    {
        private int sec = 5;
        public List<TuiTionLog> lList = new List<TuiTionLog>();
        private bool isCallAgain;
        HubConnection _connection = null;
        string url = "https://shikkhanobishRealTimeAPi.shikkhanobish.com/ShikkhanobishHub";
        public TuitionLogViewModel()
        {
            ownTag = true;
            ConnectToRealTimeApiServer();
            showTuitionInfo = false;
            GetTuitionLog();
            getStudentNumber();
            GetTuitionLog();
            isCallAgain = true;
            PerformownTagsCmd();
            StartTimer();
            TuitionEvent evetn = new TuitionEvent();
            StaticPageForPassingData.regEvent = evetn;
            evetn.clickEvent += (s, args) =>
            {
                CallForChoice(args);
            };

        }

        public async Task CallForChoice(CustomEventArgs thisArgs)
        {
            var actions = new string[] { "Yes", "No" };
            var result = await MaterialDialog.Instance.SelectActionAsync(title: thisArgs.name+ " Accepted your tuition request. Do you want to start tuition?",
                                                         actions: actions);
            Application.Current.MainPage.Navigation.PushModalAsync(new CallingPage(thisArgs.tuitionLogID));
        }
        public async Task StartTimer()
        {
            int i = 0;
            while(i == 0)
            {
                await GetTuitionLog();
                await Task.Delay(1000);
            }
        }
       
        public async Task getStudentNumber()
        {
            var stList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudent".GetJsonAsync<List<Student>>();
            studentCount = stList.Count;
        }

        public async Task GetTuitionLog()
        {
          
            isCallAgain = false;
            if (!ownTag)
            {
                lList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
                List<TuiTionLog> thisTuition = new List<TuiTionLog>();
                foreach (var tuition in lList)
                {
                    tuition.ownTag = false;
                    thisTuition.Add(tuition);
                }
                    logList = thisTuition;
            }
            else
            {
                List<TuiTionLog> thisTuition = new List<TuiTionLog>();
                lList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
                foreach(var tuition in lList)
                {
                    if(tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub1 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub2 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub3 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub4 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub5 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub6 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub7 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub8 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub9)
                    {
                        tuition.ownTag = true;
                        thisTuition.Add(tuition);
                    }
                }

                logList = thisTuition;
            }
            isCallAgain = true;
        }
        public ICommand ViewTuionInfo
        {
            get
            {
                return new Command<TuiTionLog>(async (scTuition) =>
                {
                   
                    var trList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuitionRequestCount".GetJsonAsync<List<TutionRequestCount>>();
                    scTuition.teacherNameList = new List<Teacher>();
                    foreach (var tcount in trList)
                    {
                        if(tcount.tuitionID == scTuition.tuitionLogID)
                        {
                            var twithID = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherWithID".PostJsonAsync(new { teacherID = tcount.teacherID }).ReceiveJson<Teacher>();
                            scTuition.teacherNameList.Add(twithID);
                        }
                    }
                   if(scTuition.teacherNameList.Count > 0)
                    {
                        scTuition.pendingTeacherID = scTuition.teacherNameList.Count;
                        scTuition.isPendingTeacherAvailable = true;
                    }
                    selectedTuition = scTuition;
                    showTuitionInfo = true;
                });
            }
        }
        private void PerformpopoutInfo()
        {
            showTuitionInfo = false;
        }
        private void PerformownTagsCmd()
        {
            ownTag = true;
            ownBack = Color.FromHex("#DDCBFF");
            ownLblColor = Color.FromHex("#480EB6");
            allBack = Color.Transparent;
            alllblColor = Color.Black;
        }

        private void PerformallTagsCmd()
        {
            ownTag = false;
            allBack = Color.FromHex("#DDCBFF");
            alllblColor = Color.FromHex("#480EB6");
            ownBack = Color.Transparent;
            ownLblColor = Color.Black;
        }

        private Command qsCmd1;

        public ICommand qsCmd
        {
            get
            {
                if (qsCmd1 == null)
                {
                    qsCmd1 = new Command(async => PerformqsCmd());
                }

                return qsCmd1;
            }
        }
        public async Task ConnectToRealTimeApiServer()
        {
            _connection = new HubConnectionBuilder()
                 .WithUrl(url)
                 .Build();
            await _connection.StartAsync();


            _connection.Closed += async (s) =>
            {
                await _connection.StartAsync();
            };

           
            _connection.On<string>("realTimetuitionNotiofication", async (tuitionID) =>
            {
                var lList = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getTuiTionLogNeW".GetJsonAsync<List<TuiTionLog>>();
                foreach (var tuition in lList)
                {
                    if(tuition.tuitionLogID == tuitionID)
                    {
                        if (tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub1 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub2 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub3 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub4 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub5 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub6 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub7 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub8 ||
                        tuition.subjectID == StaticPageForPassingData.thisTeacherCourseList.sub9)
                        {
                            var sh = new ShowNotification();
                            await sh.Show("New Tuition Available");
                        }
                    }

                }
            });

        }
        private async Task PerformqsCmd()
        {
            await MaterialDialog.Instance.AlertAsync(message: "টিউশন লগ উইন্ডোতে আপনি লাইভ দেখতে পারবেন কোন সাবজেক্টের উপর টিউশন কল হচ্ছে! যা আপনার টিউশন পাওয়ার সম্ভবনা বাড়িয়ে দিবে কয়েকগুন।");
        }

        private string refreshCMD1;

        public string refreshCMD { get => refreshCMD1; set => SetProperty(ref refreshCMD1, value); }

        private string refreshlbl1;

        public string refreshlbl { get => refreshlbl1; set => SetProperty(ref refreshlbl1, value); }

        private List<TuiTionLog> logList1;

        public List<TuiTionLog> logList { get => logList1; set => SetProperty(ref logList1, value); }

        private int studentCount1;

        public int studentCount { get => studentCount1; set => SetProperty(ref studentCount1, value); }

        private Command ownTagsCmd1;

        public ICommand ownTagsCmd
        {
            get
            {
                if (ownTagsCmd1 == null)
                {
                    ownTagsCmd1 = new Command(PerformownTagsCmd);
                }

                return ownTagsCmd1;
            }
        }

        private Command allTagsCmd1;

        public ICommand allTagsCmd
        {
            get
            {
                if (allTagsCmd1 == null)
                {
                    allTagsCmd1 = new Command(PerformallTagsCmd);
                }

                return allTagsCmd1;
            }
        }

        private Color ownBack1;

        public Color ownBack { get => ownBack1; set => SetProperty(ref ownBack1, value); }

        private Color ownLblColor1;

        public Color ownLblColor { get => ownLblColor1; set => SetProperty(ref ownLblColor1, value); }

        private Color allBack1;

        public Color allBack { get => allBack1; set => SetProperty(ref allBack1, value); }

        private Color alllblColor1;

        public Color alllblColor { get => alllblColor1; set => SetProperty(ref alllblColor1, value); }

        private bool ownTag1;

        public bool ownTag { get => ownTag1; set => SetProperty(ref ownTag1, value); }

        private bool showTuitionInfo1;

        public bool showTuitionInfo { get => showTuitionInfo1; set => SetProperty(ref showTuitionInfo1, value); }

        private TuiTionLog selectedTuition1;

        public TuiTionLog selectedTuition { get => selectedTuition1; set => SetProperty(ref selectedTuition1, value); }

        private Command popoutInfo1;

        public ICommand popoutInfo
        {
            get
            {
                if (popoutInfo1 == null)
                {
                    popoutInfo1 = new Command(PerformpopoutInfo);
                }

                return popoutInfo1;
            }
        }

        
    }
}