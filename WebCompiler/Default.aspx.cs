using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using JSBasic.Compiler;
using Irony.Compiler;
using System.IO;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

		convertButton.Click += new EventHandler(convertButton_Click);
		runButton.Click += new EventHandler(runButton_Click);


		string sourceFile = Request.Params["sourceCode"];
		if (!IsPostBack && !string.IsNullOrEmpty(sourceFile))
		{
			string path = Server.MapPath("App_Data/" + sourceFile + ".basic");
			if (File.Exists(path))
			{
				basicSourceTB.Text = File.ReadAllText(path);
				convertButton_Click(null, null);
				runButton_Click(null, null);
			}
		}

    }

	void convertButton_Click(object sender, EventArgs e)
	{
		CompileResult result = JavaScriptGenerator.Generate(basicSourceTB.Text);
		if (result.IsSuccessful)
		{
			javascriptSourceTB.Text = @"
function runProgram() {
	run('console', 22, 40, " + result.StartFunction + @");
}
";
			javascriptSourceTB.Text += result.JavaScript;
		}
		else
		{
			javascriptSourceTB.Text = "COMPILE ERROR" + Environment.NewLine + Environment.NewLine + result.ResultMessage;
		}
	}

	void runButton_Click(object sender, EventArgs e)
	{
		ClientScript.RegisterClientScriptBlock(GetType(), "JS", javascriptSourceTB.Text, true);
		ClientScript.RegisterStartupScript(GetType(), "Init", "runProgram();", true);
	}

}
