Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Web.HttpContext

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup

        Application.Item("SiteName") = "DBDataGrid"

        Dim objReg As New Aegon.Tools.RegistryTool
        Dim strConnString As String = objReg.GetLMValue("SOFTWARE\ADMSweb\DBDataGrid", "DBConnString")
        strConnString &= ";Application Name=" & Application.Item("SiteName")
        Application.Item("ConnString") = strConnString
        objReg = Nothing

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a application errors
    End Sub

End Class

