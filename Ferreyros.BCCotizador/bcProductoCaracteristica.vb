﻿Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcProductoCaracteristica

    Private uData As New Utiles.Datos
    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

    Public Function BuscarIdProducto(ByVal strConexion As String, ByVal obeProductoCaracteristica As beProductoCaracteristica, ByRef ListaProductoCaracteristica As List(Of beProductoCaracteristica)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaProductoCaracteristica As New daProductoCaracteristica
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaProductoCaracteristica.BuscarIdProducto(CnnSql, obeProductoCaracteristica, ListaProductoCaracteristica) Then
                    Throw New Exception(odaProductoCaracteristica.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            strError = ex.Message.ToString()
        End Try
        Return bolExito
    End Function

End Class
