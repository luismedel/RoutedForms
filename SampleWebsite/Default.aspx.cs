using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class _Default : BlinkingBits.RoutedForms.UI.Page
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        WriteInfo();
    }


    public void RenderIndex()
    {
        lblInfo.Text = "Hello from RenderIndex()!";
    }


    public void RenderAbout()
    {
        lblInfo.Text = "Hello from RenderAbout()!";
    }


    #region Info output
    private void WriteInfo()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Debug info");
        sb.AppendLine("==========");
        sb.AppendLine();

        if (string.IsNullOrEmpty (Method))
            sb.AppendLine("Called method: none");
        else
            sb.AppendFormat("Called method: {0}\n", Method);
        sb.AppendLine();

        sb.AppendLine("Arguments:");
        if (Arguments.Count == 0)
            sb.AppendLine("  No arguments supplied.");
        else
        {
            for (int i = 0; i < Arguments.Count; i++)
                sb.AppendFormat("  {0}: {1}\n", i, Arguments[i]);
        }
        sb.AppendLine();

        sb.AppendLine("Named arguments:");
        if (NamedArguments.Count == 0)
            sb.AppendLine("  No named arguments supplied.");
        else
        {
            foreach (string k in NamedArguments.Keys)
                sb.AppendFormat("  {0}: {1}\n", k, NamedArguments[k]);
        }
        sb.AppendLine();

        txtInfo.Text = sb.ToString();
    }
    #endregion
}
