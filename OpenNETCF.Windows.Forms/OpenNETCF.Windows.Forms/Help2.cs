using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OpenNETCF.Windows.Forms
{

    /// <summary>
    /// Encapsulates the PegHelp HTML Help engine.
    /// </summary>
    /// <remarks>You cannot create a new instance of the Help class.
    /// To provide Help to an application, call the static ShowHelp method.</remarks>
    public static class Help2
    {
        //name of the help exe on Pocket PC and several CE platforms
        private const string helpexecutable = "peghelp.exe";

        /// <summary>
        /// Displays the contents of the Help file at the specified URL.
        /// </summary>
        /// <param name="parent">A <see cref="System.Windows.Forms.Control"/> that identifies the parent of the Help dialog box.</param>
        /// <param name="url">The path and name of the Help file.</param>
        /// <example>The following code example demonstrates the ShowHelp method.
        /// To run this example paste the following code in a form that contains a button named Button1.
        /// Ensure the button's click event is connected to the event-handling method in this example.
        /// <code>[Visual Basic] 
        /// ' Open the Help file for the application.  
        /// Private Sub Button1_Click(ByVal sender As System.Object, _
        ///		ByVal e As System.EventArgs) Handles Button1.Click
        ///		
        ///		Help.ShowHelp(TextBox1, "\windows\myapp.htm")
        /// End Sub</code>
        /// <code>[C#]
        /// // Open the Help file for the application.  
        /// private void Button1_Click(System.Object sender, System.EventArgs e)
        /// {
        ///		Help.ShowHelp(TextBox1, "\\windows\\myapp.htm");
        ///	}</code></example>
        public static void ShowHelp(System.Windows.Forms.Control parent, string url)
        {
            //launch help with supplied filename
            Process.Start(helpexecutable, url);
        }

        /// <summary>
        /// Displays the contents of the Help file found at the specified URL for a specific topic.
        /// </summary>
        /// <param name="parent">A <see cref="System.Windows.Forms.Control"/> that identifies the parent of the Help dialog box.</param>
        /// <param name="url">The path and name of the Help file.</param>
        /// <param name="navigator">One of the <see cref="HelpNavigator"/> values.</param>
        public static void ShowHelp(System.Windows.Forms.Control parent, string url, HelpNavigator navigator)
        {
            switch (navigator)
            {
                case HelpNavigator.Find:
                    //start find applet
                    Process.Start("shfind.exe", "");
                    break;
                case HelpNavigator.TableOfContents:
                    //launch at default toc for specified helpfile
                    Process.Start(helpexecutable, url + "#Main_Contents");
                    break;
                case HelpNavigator.Topic:
                    //launch specified topic
                    Process.Start(helpexecutable, url);
                    break;
            }
        }

        /// <summary>
        /// Displays the contents of the Help file found at the specified URL for a specific topic.
        /// </summary>
        /// <param name="parent">A <see cref="System.Windows.Forms.Control"/> that identifies the parent of the Help dialog box.</param>
        /// <param name="url">The path and name of the Help file.</param>
        /// <param name="topic">The topic to display Help for.</param>
        public static void ShowHelp(System.Windows.Forms.Control parent, string url, string topic)
        {
            //launch help with supplied filename and topic
            Process.Start(helpexecutable, url + "#" + topic);
        }
        /// <summary>
        /// Displays the contents of the Help file located at the URL supplied.
        /// </summary>
        /// <param name="parent">A <see cref="System.Windows.Forms.Control"/> that identifies the parent of the Help dialog box.</param>
        /// <param name="url">The path and name of the Help file.</param>
        /// <param name="command">One of the <see cref="OpenNETCF.Windows.Forms.HelpNavigator"/> values.</param>
        /// <param name="param">The anchor name of the topic to display</param>
        public static void ShowHelp(System.Windows.Forms.Control parent, string url, HelpNavigator command, string param)
        {
            switch (command)
            {
                case HelpNavigator.Find:
                    //start find applet
                    Process.Start("shfind.exe", "");
                    break;
                case HelpNavigator.TableOfContents:
                    //launch at default toc for specified helpfile
                    Process.Start(helpexecutable, url + "#Main_Contents");
                    break;
                case HelpNavigator.Topic:
                    //launch specified topic
                    Process.Start(helpexecutable, url + "#" + param);
                    break;
            }
        }
    }
}
