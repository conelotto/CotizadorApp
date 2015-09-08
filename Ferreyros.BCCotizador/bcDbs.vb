Imports Ferreyros.DACotizador
Imports System.Data.OleDb
Imports Ferreyros.BECotizador

Public Class bcDbs
    Private pr_strError As String
    Private _daDbs As daDbs = Nothing
    Public ReadOnly Property ErrorDes() As String
        Get
            Return pr_strError
        End Get
    End Property

#Region "Modelo"
    Public Function BuscarModelo(ByVal strConexion As String, ByVal Libreria As String, ByVal Codigo As String, _
                                 ByVal Version As String, ByVal Descripcion As String, ByRef dtrModelo As DataRow) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.BuscarModelo(cnnSql, Libreria, Codigo, Version, Descripcion, dtrModelo) Then
                    Throw New Exception(oDbs.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function BuscarModelo(ByVal strConexion As String, ByVal Libreria As String, ByVal ValorBusqueda As String, _
                                 ByRef dstModelo As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.BuscarModelo(cnnSql, Libreria, ValorBusqueda, dstModelo) Then
                    Throw New Exception(oDbs.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function BuscarSoloModelo(ByVal strConexion As String, ByVal Libreria As String, ByVal Codigo As String, _
                                 ByRef dstModelo As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.BuscarSoloModelo(cnnSql, Libreria, Codigo, dstModelo) Then
                    Throw New Exception(oDbs.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function ObtenerModelo(ByVal strConexion As String, ByVal Libreria As String, ByVal Codigo As String, _
                                     ByVal IncluirBuscarNombre As Boolean) As String
        Dim dstModelo As New DataSet
        Dim Modelo As String = String.Empty
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.BuscarSoloModelo(cnnSql, Libreria, Codigo, dstModelo) Then
                    Throw New Exception(oDbs.ErrorDes)
                Else
                    If Not dstModelo.Tables(0).Rows.Count.Equals(0) Then
                        Modelo = dstModelo.Tables(0).Rows(0)("DS8").ToString
                    End If
                End If
            End Using
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return Modelo
    End Function
#End Region

#Region "Cliente"
    Public Function BuscarCliente(ByVal strConexion As String, ByVal Libreria As String, _
                                 ByVal ValorBusqueda As String, ByVal Corporacion As String, _
                                ByVal Compañia As String, ByRef dstCliente As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.BuscarCliente(cnnSql, Libreria, ValorBusqueda, Corporacion, Compañia, dstCliente) Then
                    Throw New Exception(oDbs.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function BuscarCliente(ByVal strConexion As String, ByVal Libreria As String, _
                                 ByVal CodigoCliente As String, ByVal Corporacion As String, _
                                ByVal Compañia As String, ByRef dtrCliente As DataRow) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.BuscarCliente(cnnSql, Libreria, CodigoCliente, Corporacion, Compañia, dtrCliente) Then
                    Throw New Exception(oDbs.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function
#End Region

#Region "Contacto"
    Public Function BuscarContacto(ByVal strConexion As String, ByVal Libreria As String, ByVal CodigoContacto As String, _
                                 ByVal CodigoCliente As String, ByRef dtrContacto As DataRow) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.BuscarContacto(cnnSql, Libreria, CodigoContacto, CodigoCliente, dtrContacto) Then
                    Throw New Exception(oDbs.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function BuscarContactoCliente(ByVal strConexion As String, ByVal Libreria As String, ByVal CodigoCliente As String, _
                                 ByVal ValorBusqueda As String, ByRef dstContacto As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.BuscarContactoCliente(cnnSql, Libreria, CodigoCliente, ValorBusqueda, dstContacto) Then
                    Throw New Exception(oDbs.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function
#End Region

#Region "EntidadMaestraDbs"
    Public Function BuscarEntidadMaestraDbs(ByVal strConexion As String, ByVal Libreria As String, _
                                            ByVal EntidadMaestraDbs As List(Of String), ByRef dstEntidadMaestraDbs As DataSet, _
                                            Optional ByVal Codigo As String = "") As Boolean
        Dim blnResultado As Boolean
        Dim cnnSql As OleDbConnection = Nothing
        Try
            Dim oDbs As New daDbs
            cnnSql = New OleDbConnection(strConexion)
            cnnSql.Open()
            For Each Entidad As String In EntidadMaestraDbs
                If Not oDbs.BuscarEntidadMaestraDbs(cnnSql, Libreria, Entidad, dstEntidadMaestraDbs, Codigo) Then
                    Throw New Exception(oDbs.ErrorDes)
                End If
            Next
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        Finally
            If cnnSql.State = ConnectionState.Open Then cnnSql.Close()
        End Try
        Return blnResultado
    End Function

    Public Sub ListarEntidadMaestraDbs(ByVal oConexion As String, ByVal oLibreria As String, ByVal oTabla As beTablaMaestra, ByRef oValidacion As beValidacion, ByRef oDetalle As List(Of beTablaMaestra))

        If String.IsNullOrEmpty(oConexion) OrElse String.IsNullOrEmpty(oLibreria) OrElse oTabla Is Nothing Then
            Return
        End If

        Try
            _daDbs = New daDbs
            _daDbs.ListarEntidadMaestraDbs(oConexion, oLibreria, oTabla, oValidacion, oDetalle)
        Catch ex As Exception
            oValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub
#End Region

#Region "TipoCambio"
    ''' <summary>
    ''' Obtener Tipo de Cambio del DBS
    ''' </summary>
    ''' <param name="strConexion">Cadena de conección</param>
    ''' <param name="Libreria">Librería del AS400</param>
    ''' <param name="Corporacion">Código de la corporación</param>
    ''' <param name="Compañia">Código de la compañia</param>
    ''' <param name="Moneda">Código de la moneda</param>
    ''' <param name="Fecha">Fecha a consultar (Formato yyyyMMdd)</param>
    ''' <param name="MasReciente">Recupera el valor mas recinete de on encontrar en la fecha indicada</param>
    ''' <param name="dtrTipoCambio">Datarow devuelto</param>
    ''' <returns>Datarow</returns>
    ''' <remarks></remarks>
    Public Function ObtenerTipoCambio(ByVal strConexion As String, ByVal Libreria As String, _
                                      ByVal Corporacion As String, ByVal Compañia As String, _
                                      ByVal Moneda As String, ByVal Fecha As String, _
                                      ByVal MasReciente As Boolean, ByRef dtrTipoCambio As DataRow) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oDbs As New daDbs
            Using cnnSql As New OleDbConnection(strConexion)
                cnnSql.Open()
                If Not oDbs.ObtenerTipoCambio(cnnSql, Libreria, Corporacion, Compañia, Moneda, Fecha, dtrTipoCambio) Then
                    If dtrTipoCambio Is Nothing And MasReciente Then
                        If Not oDbs.ObtenerTipoCambio(cnnSql, Libreria, Corporacion, Compañia, Moneda, dtrTipoCambio) Then
                            Throw New Exception(oDbs.ErrorDes)
                        End If
                    End If
                Else
                    Throw New Exception(oDbs.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function
#End Region

End Class