Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Specialized

Public Class SqlDataProvider

#Region "Private Declarations"

    Private _connection As System.Data.SqlClient.SqlConnection
    Private _transaction As SqlTransaction
    Private _connectionString As String

    Private Function GetConnection() As SqlConnection
        Dim conn As New SqlConnection(_connectionString)
        Try
            Dim previousConnectionState As ConnectionState = conn.State
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return conn
    End Function

    Private Sub BuildParameters(ByRef sqlParams As NameValueCollection, ByRef sqlCommand As SqlCommand)
        SqlCommandBuilder.DeriveParameters(sqlCommand)
        If Not sqlParams Is Nothing Then
            For Each paramName As String In sqlParams.Keys
                Dim sqlParam As SqlParameter = sqlCommand.Parameters.Item(paramName)

                If Not sqlParam Is Nothing AndAlso sqlParams(paramName) > "" Then
                    sqlParam.Value = sqlParams(paramName)
                End If

            Next
        End If
    End Sub

    Private Function BuildOutputParameters(ByRef sqlCommand As SqlCommand) As NameValueCollection
        Dim sqlOutputParams As New NameValueCollection

        For Each sqlParam As SqlParameter In sqlCommand.Parameters
            If sqlParam.Direction = ParameterDirection.InputOutput _
             Or sqlParam.Direction = ParameterDirection.Output _
             Or sqlParam.Direction = ParameterDirection.ReturnValue Then

                If IsDBNull(sqlParam.Value) Then
                    sqlOutputParams.Add(sqlParam.ParameterName, Nothing)
                Else
                    sqlOutputParams.Add(sqlParam.ParameterName, sqlParam.Value)
                End If
            End If
        Next
        Return sqlOutputParams
    End Function
#End Region

#Region "Constructor"

    Public Sub New(ByVal connectionString As String)
        _connectionString = connectionString
    End Sub

#End Region

#Region "Public Methods"

    Public Sub BeginTransaction()
        If _connection Is Nothing Then
            _connection = GetConnection()
        End If

        If _connection.State <> ConnectionState.Open Then
            _connection.Open()
        End If

        _transaction = _connection.BeginTransaction
    End Sub

    Public Sub CommitTransaction()
        If Not _transaction Is Nothing Then
            _transaction.Commit()
            _connection.Close()
            _transaction = Nothing
            _connection = Nothing
        End If
    End Sub

    Public Sub RollbackTransaction()
        If Not _transaction Is Nothing Then
            _transaction.Rollback()
            _connection.Close()
            _transaction = Nothing
            _connection = Nothing
        End If
    End Sub

    Public Function Execute(ByVal sqlInParams As System.Collections.Specialized.NameValueCollection, ByVal cmdText As String, ByVal cmdType As System.Data.CommandType, Optional ByRef sqlOutParams As System.Collections.Specialized.NameValueCollection = Nothing) As Integer

        Dim intRetVal As Integer = 0

        Try
            If _connection Is Nothing Then
                _connection = GetConnection()
            End If

            If _connection.State <> ConnectionState.Open Then
                _connection.Open()
            End If

            Dim cmd As New SqlCommand
            cmd.CommandText = cmdText
            cmd.CommandType = cmdType
            cmd.Connection = _connection

            If Not _transaction Is Nothing Then
                cmd.Transaction = _transaction
            End If

            BuildParameters(sqlInParams, cmd)

            intRetVal = cmd.ExecuteNonQuery()

            sqlOutParams = BuildOutputParameters(cmd)
        Catch sqlEx As SqlException
            Throw sqlEx
        Catch ex As Exception
            Throw ex
        Finally
            If _transaction Is Nothing Then
                _connection.Close()
                _connection = Nothing
            End If
        End Try

        Return intRetVal
    End Function

    Public Function ExecuteDataset(ByVal sqlInParams As System.Collections.Specialized.NameValueCollection, ByVal cmdText As String, ByVal cmdType As System.Data.CommandType, Optional ByRef sqlOutParams As System.Collections.Specialized.NameValueCollection = Nothing) As System.Data.DataSet

        Dim dsOut As DataSet = Nothing
        Dim sqlDA As SqlDataAdapter

        Try
            If _connection Is Nothing Then
                _connection = GetConnection()
            End If

            If _connection.State <> ConnectionState.Open Then
                _connection.Open()
            End If

            Dim cmd As New SqlCommand
            cmd.CommandText = cmdText
            cmd.CommandType = cmdType
            cmd.Connection = _connection

            If Not _transaction Is Nothing Then
                cmd.Transaction = _transaction
            End If

            BuildParameters(sqlInParams, cmd)

            sqlDA = New SqlDataAdapter
            dsOut = New DataSet
            sqlDA.SelectCommand = cmd
            sqlDA.Fill(dsOut)

            sqlOutParams = BuildOutputParameters(cmd)
        Catch sqlEx As SqlException
            Throw sqlEx
        Catch ex As Exception
            Throw ex
        Finally
            If _transaction Is Nothing Then
                _connection.Close()
                _connection = Nothing
            End If
        End Try

        Return dsOut
    End Function

    Public Function ExecuteScalar(ByVal sqlInParams As System.Collections.Specialized.NameValueCollection, ByVal cmdText As String, ByVal cmdType As System.Data.CommandType, Optional ByRef sqlOutParams As System.Collections.Specialized.NameValueCollection = Nothing) As Object

        Try
            If _connection Is Nothing Then
                _connection = GetConnection()
            End If

            If _connection.State <> ConnectionState.Open Then
                _connection.Open()
            End If

            Dim cmd As New SqlCommand
            cmd.CommandText = cmdText
            cmd.CommandType = cmdType
            cmd.Connection = _connection

            If Not _transaction Is Nothing Then
                cmd.Transaction = _transaction
            End If

            BuildParameters(sqlInParams, cmd)

            Return cmd.ExecuteScalar()
        Catch sqlEx As SqlException
            Throw sqlEx
        Catch ex As Exception
            Throw ex
        Finally
            If _transaction Is Nothing Then
                _connection.Close()
                _connection = Nothing
            End If
        End Try

        Return Nothing
    End Function

#End Region

End Class

