using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace edu.neu.ccis.rasala.controls
{
    /// <summary>
    /// The original design and code for this control was presented in:
    ///   Matthew MacDonald, Adam Freeman, and Mario Szpuszta 
    ///   Pro ASP.NET 4 in C# 2010 
    ///   Apress, 2010, 978-1-4302-2529-4
    /// on pages 1127-1129.
    /// 
    /// However, this design failed because it did not handle view state
    /// on postback.
    /// 
    /// 
    /// I did a google search for CompositeControl.
    /// 
    /// This led to a blog post from Scott Guthrie in 2006:
    /// 
    /// http://weblogs.asp.net/scottgu/archive/2006/02/14/438234.aspx
    /// 
    /// 
    /// Dave Reed posted a comment on this blog that is key to a solution.
    /// 
    /// Reed strongly recommended using the state of the nested controls
    /// directly rather than manipulating the view state of the composite.
    /// 
    /// The importance of this technique is that view state is updated on
    /// postback by normal ASP.NET mechanisms.
    /// 
    /// The code by Dave Reed entirely replaces the code from Pro ASP.NET 4.
    /// 
    /// 
    /// I added properties to permit setting CSS styles directly.
    /// </summary>
    public class TitledTextBox : CompositeControl
	{
        /// Instantiate the constituent controls with member variables so:
        ///   The controls are accessible in all methods
        ///   The controls may be returned by properties

        protected Label label;

        protected TextBox textbox;


        /// Constructor
        /// Ensure that the controls are created and installed
        public TitledTextBox()
        {
            EnsureChildControls();  /// Calls CreateChildControls()
        }


        /// Get the Label object
        public Label TheLabel
        {
            get { return label; }
        }


        /// Get the TextBox object
        public TextBox TheTextBox
        {
            get { return textbox; }
        }


        /// Get or set the label text
		public string Title
		{
            get
            {
                return label.Text ?? "";
            }
			set 
            {
                label.Text = value ?? "";
            }
		}


        /// Get or set the text box text
		public string Text
		{
            get
            {
                return textbox.Text ?? "";
            }
            set
            {
                textbox.Text = value ?? "";
            }
        }


        /// Get or set the label css class
        public string LabelCssClass
        {
            get
            {
                return label.CssClass ?? "";
            }
            set
            {
                label.CssClass = value ?? "";
            }
        }


        /// Get or set the text box css class
        public string TextBoxCssClass
        {
            get
            {
                return textbox.CssClass ?? "";
            }
            set
            {
                textbox.CssClass = value ?? "";
            }
        }


        /// Event handler object for attaching
        /// external text changed handlers.
        public event EventHandler TextChanged;


        /// Create and add the child controls
        /// to the Controls collection
		protected override void CreateChildControls()
		{
			/// Add the label
			label = new Label();
            label.ID = "TheLabel";
			Controls.Add(label);

            /// This line below was taken from Pro ASP.NET 4.
            ///
            /// We decided that the markup should not have gratuitous space.
			/// Spacing is better done by adding a blank in the title or via CSS.
            ///
			/// Controls.Add(new LiteralControl("&nbsp;"));

			/// Add the text box
			textbox = new TextBox();
            textbox.ID = "TheTextBox";
			Controls.Add(textbox);

            /// Arranges to fire the external text changed handlers
            /// on text changed events in the text box
            textbox.TextChanged += new EventHandler(OnTextChanged);
        }


        /// Method to fire the external text changed handlers
		protected virtual void OnTextChanged(object sender, EventArgs e)
		{
			if (TextChanged != null)
				TextChanged(this, e);
		}


        /// <summary>
        /// Copy the source data to the target.
        /// </summary>
        public static void Copy(TitledTextBox source, TitledTextBox target)
        {
            target.Text = source.Text;
        }

    }
}
