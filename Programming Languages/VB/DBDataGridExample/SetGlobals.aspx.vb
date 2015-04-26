
Partial Class SetGlobals
    Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not Page.IsPostBack Then
			LoadGrids()
		End If
	End Sub

	Protected Sub btnSetGlobals_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetGlobals.Click

        Application.Item("SiteName") = "DBDataGrid"

        Dim objReg As New Aegon.Tools.RegistryTool
        Dim strConnString As String = objReg.GetLMValue("SOFTWARE\ADMSweb\DBDataGrid", "DBConnString")
        strConnString &= ";Application Name=" & Application.Item("SiteName")
        Application.Item("ConnString") = strConnString
        objReg = Nothing

        LoadGrids()

	End Sub

	Private Sub LoadGrids()
		Dim intNdx As Integer

		Dim htbApplication As New Hashtable
        Dim strMaskedApplicationItem As String = Nothing
        Dim strMaskedSessionItem As String = Nothing

		lblAppItems.Text = Application.Count.ToString
        For intNdx = 0 To Application.Count - 1
            ' Mask SQL database UID and PWD entries
            strMaskedApplicationItem = Application.Item(intNdx).ToString
            strMaskedApplicationItem = strMaskedApplicationItem.Replace("UID=PlanoWebApp", "UID=******")
            strMaskedApplicationItem = strMaskedApplicationItem.Replace("PWD=webapp2000", "PWD=******")
            htbApplication.Add(Application.GetKey(intNdx), strMaskedApplicationItem)
        Next
		rptApplication.DataSource = htbApplication
		rptApplication.DataBind()

		Dim htbSession As New Hashtable
		lblSessItems.Text = Session.Count.ToString
		For intNdx = 0 To Session.Count - 1
            ' Mask SQL database UID and PWD entries
            strMaskedSessionItem = Session.Item(intNdx).ToString
            strMaskedSessionItem = strMaskedSessionItem.Replace("UID=PlanoWebApp", "UID=******")
            strMaskedSessionItem = strMaskedSessionItem.Replace("PWD=webapp2000", "PWD=******")
            htbSession.Add(Session.Keys(intNdx), strMaskedSessionItem)
		Next
		rptSession.DataSource = htbSession
		rptSession.DataBind()

	End Sub

	Protected Sub btnResetSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSession.Click
		Session.Abandon()
    End Sub

End Class
