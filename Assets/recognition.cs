using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Windows.Speech;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

//using UnityEngine.AudioClip;

namespace AssemblyCSharp
{
public class recognition : MonoBehaviour {
		public Animator anim1;
		// Use this for initialization
		//public AudioClip wordpad;

		public AudioSource source;
		public AudioClip wordpad;
		public AudioClip notepad;
        public AudioClip google;
        public AudioClip calc;
        public AudioClip comp;
        public AudioClip default1;
        public string comm;
	KeywordRecognizer keyrec;
	Process proc=new Process();
	//GrammarRecognizer gram;
	Dictionary<string, System.Action> keywords= new Dictionary<string, System.Action>();
	List<Word> words = new List<Word>();
        [DllImport("user32")]
        public static extern void LockWorkStation();
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);


        void Start () { 
			anim1 = GetComponent<Animator> ();
			source = GetComponent<AudioSource>();
            

            load ();
		

			//source =GetComponent<AudioSource>();
			//wordpad = (AudioClip)Resources.Load ("wordpad");
					
		keyrec = new KeywordRecognizer (keywords.Keys.ToArray ());
			keyrec.OnPhraseRecognized+= KeywordRecogniserOnPhraseRecognised;
			keyrec.Start(); 
	
	}

        void  KeywordRecogniserOnPhraseRecognised(PhraseRecognizedEventArgs args)
	{
		System.Action keywordAction;

				if (keywords.TryGetValue (args.text, out keywordAction)) {
			
			//string com = args.Text.ToString;

					keywordAction.Invoke ();
					

			
			

					///return 1;


				}
       }
        void calci()
        {

            anim1.SetBool("cal", true);
           


            Invoke("execute1", 4);

        }

		void  execute(string com)
		{
	

			comm = com;
            print(com);
            if (comm == "open calculator")
            {
                calci();
                calc = (AudioClip)Resources.Load("calculator");
                source.PlayOneShot(calc);

            }
            else
            {


                if (comm == "open notepad")
                {
                    //notepad = (AudioClip)Resources.Load ("notepad");

                    //source.clip = notepad;

                    anim1.SetBool("word", true);
                    notepad = (AudioClip)Resources.Load("notepad");

                    source.clip = notepad;
                    source.Play();
                    //source.PlayDelayed (1);
                    //source.Play();
                    Invoke("execute1", 4);

                    print("i have set false");

                }
                else if (comm == "open wordpad")
                {
                    //notepad = (AudioClip)Resources.Load ("notepad");

                    //source.clip = notepad;

                    anim1.SetBool("word", true);
                    wordpad = (AudioClip)Resources.Load("wordpad");

                    source.clip = wordpad;
                    source.Play();
                    //source.PlayDelayed (1);
                    //source.Play();
                    Invoke("execute1", 4);

                    print("i have set false");

                }
                else if (comm == "open google")
                {


                    anim1.SetBool("google", true);
                    google = (AudioClip)Resources.Load("google");

                    source.clip = google;
                    source.Play();
                    //source.PlayDelayed (1);
                    //source.Play();
                    Invoke("execute1", 3);
                }
                /*else if (comm == "open calculator") 
                {

                    anim1.SetBool("cal", true);
                    calc = (AudioClip)Resources.Load("calc");
                    source.PlayOneShot(calc);


                    Invoke ("execute1", 4);
                }*/
                else if (comm == "open computer")
                {


                    anim1.SetBool("comp", true);
                    comp = (AudioClip)Resources.Load("computer");

                    source.clip = comp;
                    source.Play();
                    // source.PlayDelayed (1);

                    //source.Play();

                }
                else if (comm == "shutdown")
                {


                    Process.Start("shutdown", "/s /t 0");
                }
                else if (comm == "lock system")
                {

                    LockWorkStation();

                }
                else if (comm == "restart")
                {


                    Process.Start("shutdown", "/r /t 0");
                }
                else if (comm == "log off system")
                {


                    ExitWindowsEx(0, 0);
                }
                else
                {
                    anim1.SetBool("def", true);
                    default1 = (AudioClip)Resources.Load("default");
                    source.clip = default1;

                    source.Play();

                    Invoke("execute1", 4);
                }
            }

		
	


								
				//print ("i am at procstart");
			
				//return 1;
				 			
       }


		void execute1()
		{
			print("i am");
			var cmd = words.Where (c => c.Text ==comm).First ();
			if (cmd.IsShellCommand) {



                if (cmd.Text.Contains("close"))
                {
                    Process[] pro = Process.GetProcesses();
                    for (int i = 0; i < pro.Length; i++)
                    {
                        if (pro[i].ProcessName == cmd.AttachedText)
                        {
                            pro[i].Kill();
                            anim1.SetBool("def", false);
                            break;
                        }

                    }


                }
                else
                {
                    proc.StartInfo.FileName = cmd.AttachedText;
                    proc.EnableRaisingEvents = false;
                    

                    proc.Start();
                }
                anim1.SetBool("word", false);
                anim1.SetBool("note", false);
                anim1.SetBool("comp", false);
                anim1.SetBool("cal", false);
                anim1.SetBool("google", false);
                anim1.SetBool("def", false);



            }
           else
            {
                if(comm.Contains("time"))
                {


                    DateTime time = DateTime.Now;
                    //  DateTime date = DateTime.Today;
                    string t = time.ToString("h mm tt");
                    // string t1 =date.ToString("dd mm yyyy");
                    //syn.SpeakAsync("the time is " + t);
                    //  syn.SpeakAsync("today's " + t1);
                    // Application.Exit();
                    print(t);

                }
                else if (comm.Contains("date"))
                {
                    DateTime date = DateTime.Today;
                    string t1 = date.ToString("dd mm yyyy");
                    print(t1);



                }
                anim1.SetBool("def", false);
            }

		}


        void load()
		{

			string[] text= System.IO.File.ReadAllLines("example.txt");
            foreach (string line in text)

            {
                var parts = line.Split(new char[] { '|' });

                words.Add(new Word() { Text = parts[0], AttachedText = parts[1], IsShellCommand = (parts[2] == "true") });
                keywords.Add(parts[0], () =>
                {

                    execute(parts[0]);
                    //print(sup);

                });

            }
             
                keywords.Add("shutdown", () => {

                    execute("shutdown");
                    //print(sup);

                });
                keywords.Add("restart", () => {

                    execute("restart");
                    //print(sup);

                });
           
            keywords.Add("lock system", () => {

                    execute("lock system");
                    //print(sup);

                });
            keywords.Add("log off system", () => {

                execute("log off system");
                //print(sup);

            });
            //print (parts [0]);




        }
	
}
}