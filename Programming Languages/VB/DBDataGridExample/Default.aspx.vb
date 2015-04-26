Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports MyCustomColumn
Imports MyCustomColumn.MyCustomColumn

Partial Class _Default
    Inherits System.Web.UI.Page

    Dim m_Connstring As String = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_Connstring = Application.Item("ConnString")

        'add state prompt
        ddlState.DataBind()
        ddlState.Items.Insert(0, New ListItem("Select State", ""))

        'add relationship prompt
        ddlRelationship.DataBind()
        ddlRelationship.Items.Insert(0, New ListItem("Select Relationship", ""))

        If (Not IsPostBack) Then
            BindData()
        End If

    End Sub

    Private Sub BindData()

        Dim MyConnection As SqlConnection
        Dim MyAdapter As SqlDataAdapter
        Dim MyDataSet As DataSet

        MyConnection = New SqlConnection()
        MyConnection.ConnectionString = m_Connstring

        MyAdapter = New SqlDataAdapter("SELECT * FROM DBGrid_Demo", MyConnection)
        MyDataSet = New DataSet
        MyAdapter.Fill(MyDataSet, "Contacts")

        dgContacts.DataSource = MyDataSet
        dgContacts.DataBind()

        'MyAdapter = New SqlDataAdapter("SELECT * FROM DBGrid_Demo_Locations", MyConnection)
        'MyAdapter.Fill(MyDataSet, "Locations")

        'Dim DDC As DropDownColumn
        'DDC = CType(dgContacts.Columns(3), DropDownColumn)
        'DDC.DataSource = MyDataSet

        MyConnection.Dispose()
        MyAdapter.Dispose()
        MyConnection.Dispose()

    End Sub

    Protected Sub dgContacts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgContacts.ItemDataBound

        If e.Item.ItemType = ListItemType.EditItem Then
            Dim DRV As DataRowView = CType(e.Item.DataItem, DataRowView)
            Dim CurrentLocation As String = DRV("Location")
            Dim DDL As DropDownList = CType(e.Item.Cells(3).Controls(1), DropDownList)
            Dim SQL As String = "SELECT CONVERT(nvarchar, LocationID) + ' - ' + Location as LocationID, Location FROM DBGrid_Demo_Locations ORDER BY Location"
            Dim DA As SqlDataAdapter = New SqlDataAdapter(SQL, m_Connstring)
            Dim DS As New DataSet
            Dim item As ListItem
            DA.Fill(DS, "Locations")
            DDL.DataSource = DS.Tables("Locations").DefaultView
            DDL.DataTextField = "LocationID"
            DDL.DataValueField = "Location"
            DDL.DataBind()
            item = DDL.Items.FindByValue(CurrentLocation)
            If Not item Is Nothing Then item.Selected = True
        End If

    End Sub

    Protected Sub dgContacts_InsertCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs)

        Dim intReturn As Integer = 0

        If e.CommandName = "Insert" Then

            'FirstName
            Dim firstName As String = (CType(e.Item.Cells(1).Controls(1), TextBox)).Text
            'LastName
            Dim lastName As String = (CType(e.Item.Cells(2).Controls(1), TextBox)).Text
            'Location
            Dim location As String = (CType(e.Item.Cells(3).Controls(1), TextBox)).Text
            'PhoneNbr
            Dim phoneNumber As String = (CType(e.Item.Cells(4).Controls(1), TextBox)).Text

            Try

                Dim sqlData As New SqlDataProvider(Application.Item("ConnString"))

                Dim inParams As New NameValueCollection
                inParams.Add("@FirstName", firstName)
                inParams.Add("@LastName", lastName)
                inParams.Add("@Location", location)
                inParams.Add("@PhoneNumber", phoneNumber)

                intReturn = sqlData.Execute(inParams, "sp_InsertContacts", CommandType.StoredProcedure)

                dgContacts.EditItemIndex = -1
                BindData()
                'Dim rowsAffected As Integer = 1

            Catch ex As Exception

            End Try

        End If

    End Sub

    Protected Sub dgContacts_CancelCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs)

        dgContacts.ShowFooter = True
        dgContacts.EditItemIndex = -1
        BindData()

    End Sub

    Protected Sub dgContacts_EditCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs)

        dgContacts.ShowFooter = False
        dgContacts.EditItemIndex = e.Item.ItemIndex
        BindData()

    End Sub

    Protected Sub dgContacts_UpdateCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs)

        'ID
        Dim contactID As String = (CType(e.Item.Cells(0).Controls(1), Label)).Text
        'FirstName
        Dim firstName As String = (CType(e.Item.Cells(1).Controls(1), TextBox)).Text
        'LastName
        Dim lastName As String = (CType(e.Item.Cells(2).Controls(1), TextBox)).Text
        'Location
        Dim DDL As DropDownList = CType(e.Item.Cells(3).Controls(1), DropDownList)
        Dim location As String = DDL.SelectedValue
        'Dim location As String = (CType(e.Item.Cells(3).Controls(1), TextBox)).Text
        'PhoneNbr
        Dim phoneNumber As String = (CType(e.Item.Cells(4).Controls(1), TextBox)).Text

        Try

            Dim sqlData As New SqlDataProvider(Application.Item("ConnString"))

            Dim inParams As New NameValueCollection
            inParams.Add("@ContactID", contactID)
            inParams.Add("@FirstName", firstName)
            inParams.Add("@LastName", lastName)
            inParams.Add("@Location", location)
            inParams.Add("@PhoneNumber", phoneNumber)

            sqlData.Execute(inParams, "sp_UpdateContacts", CommandType.StoredProcedure)

            dgContacts.EditItemIndex = -1
            BindData()
            Dim rowsAffected As Integer = 1

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub dgContacts_DeleteCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs)

        Dim intReturn As Integer = 0
        Dim ID As Integer = CInt(Fix(dgContacts.DataKeys(CInt(Fix(e.Item.ItemIndex)))))

        Try

            Dim sqlData As New SqlDataProvider(Application.Item("ConnString"))

            Dim inParams As New NameValueCollection
            inParams.Add("@ContactID", ID)

            intReturn = sqlData.Execute(inParams, "sp_DeleteContacts", CommandType.StoredProcedure)

        Catch ex As Exception

        End Try

        BindData()

    End Sub

    Protected Sub dgContacts_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgContacts.PageIndexChanged

        dgContacts.CurrentPageIndex = e.NewPageIndex
        BindData()

    End Sub

End Class
