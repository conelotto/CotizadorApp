Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador
Imports System.Globalization

Public Class bcTablaMaestra

    Private pr_strError As String
    Private dTablaMaestra As daTablaMaestra = Nothing
    Public ReadOnly Property ErrorDes() As String
        Get
            Return pr_strError
        End Get
    End Property

    Public Function BuscarId(ByVal strConexion As String, ByVal eTablaMaestra As beTablaMaestra, ByRef dtrTablaMaestra As DataRow) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oTablaMaestra As New daTablaMaestra
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not oTablaMaestra.BuscarId(cnnSql, eTablaMaestra, dtrTablaMaestra) Then
                    Throw New Exception(oTablaMaestra.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function BuscarGrupo(ByVal strConexion As String, ByVal leTablaMaestra As List(Of beTablaMaestra), _
                           ByRef dstTablaMaestra As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oTablaMaestra As New daTablaMaestra
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                For Each eTabla In leTablaMaestra
                    If Not oTablaMaestra.BuscarGrupo(cnnSql, eTabla, dstTablaMaestra) Then
                        Throw New Exception(oTablaMaestra.ErrorDes)
                    End If
                Next
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Function BuscarGrupo(ByVal strConexion As String, ByVal eTablaMaestra As beTablaMaestra, _
                           ByRef dstTablaMaestra As DataSet) As Boolean
        Dim blnResultado As Boolean
        Try
            Dim oTablaMaestra As New daTablaMaestra
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                If Not oTablaMaestra.BuscarGrupo(cnnSql, eTablaMaestra, dstTablaMaestra) Then
                    Throw New Exception(oTablaMaestra.ErrorDes)
                End If
            End Using
            blnResultado = True
        Catch ex As Exception
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Sub ListarSeccionesCotizaciones(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beTablaMaestra))

        Try
            dTablaMaestra = New daTablaMaestra
            dTablaMaestra.ListarSeccionesCotizaciones(eConexion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub


    Public Sub MantenimientoCriterio(ByVal eConexion As String, ByVal eTablaMaestra As beTablaMaestra, ByRef eValidacion As beValidacion)

        Try
            dTablaMaestra = New daTablaMaestra
            dTablaMaestra.MantenimientoCriterio(eConexion, eTablaMaestra, eValidacion)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub MantenimientoSeccion(ByVal eConexion As String, ByVal eTablaMaestra As beTablaMaestra, ByVal l_SeccionCriterio As List(Of beTablaMaestra), ByRef eValidacion As beValidacion)

        Try
            dTablaMaestra = New daTablaMaestra
            dTablaMaestra.MantenimientoSeccion(eConexion, eTablaMaestra, l_SeccionCriterio, eValidacion)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub ListarCriteriosPorSeccion(ByVal strConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beTablaMaestra))
        Try

            Dim CodigoSeccion As String = eValidacion.flag
            dTablaMaestra = New daTablaMaestra
            dTablaMaestra.ListarCriteriosPorSeccion(strConexion, eValidacion, lResult)

        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try
    End Sub

    Public Sub MantenimientoListaSeccion(ByVal eConexion As String, ByVal l_ListaSeccion As List(Of beTablaMaestra), ByRef eValidacion As beValidacion)

        Try
            dTablaMaestra = New daTablaMaestra
            dTablaMaestra.MantenimientoListaSeccion(eConexion, l_ListaSeccion, eValidacion)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub

    Public Sub ListarSeccionesxUsuario(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beTablaMaestra))

        Try
            dTablaMaestra = New daTablaMaestra
            dTablaMaestra.ListarSeccionesxUsuario(eConexion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub


    Public Sub MarcadorListar(ByVal eConexion As String, ByRef eValidacion As beValidacion, ByRef lResult As List(Of beMarcador))

        Try
            dTablaMaestra = New daTablaMaestra
            dTablaMaestra.MarcadorListar(eConexion, eValidacion, lResult)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
        End Try

    End Sub
    Public Function MarcadorCotizacionInsertar(ByVal strConexion As String, ByRef obeMarcadorCotizacion As beMarcadorCotizacion) As Boolean
        pr_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            Dim odaTablaMaestra As New daTablaMaestra
            If Not odaTablaMaestra.MarcadorCotizacionInsertar(CnnSql, obeMarcadorCotizacion) Then
                Throw New Exception(odaTablaMaestra.ErrorDes)
            End If
            bolExito = True
        Catch ex As Exception
            pr_strError = ex.Message
        End Try
        Return bolExito
    End Function
    Public Function MarcadorCotizacionActualizar(ByVal strConexion As String, ByVal obeMarcadorCotizacion As beMarcadorCotizacion) As Boolean
        pr_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Dim CnnSql As New SqlConnection(strConexion)
            CnnSql.Open()
            Dim odaTablaMaestra As New daTablaMaestra
            If Not odaTablaMaestra.MarcadorCotizacionActualizar(CnnSql, obeMarcadorCotizacion) Then
                Throw New Exception(odaTablaMaestra.ErrorDes)
            End If
            bolExito = True
        Catch ex As Exception
            pr_strError = ex.Message
        End Try
        Return bolExito
    End Function
    Public Sub MarcadorBuscarIdArchivoConfig(ByVal eConexion As String, ByRef ebeMarcadorCotizacion As beMarcadorCotizacion, ByRef lResult As List(Of beMarcadorCotizacion))

        Try
            dTablaMaestra = New daTablaMaestra
            dTablaMaestra.MarcadorBuscarIdArchivoConfig(eConexion, ebeMarcadorCotizacion, lResult)
        Catch ex As Exception
            pr_strError = ex.Message
        End Try

    End Sub

    Public Function MarcadorMantenimiento(ByVal strConexion As String, ByVal lista As List(Of beMarcadorCotizacion)) As String
        Dim strReturn As String = "0"
        Dim Connection As New SqlConnection(strConexion)
        Connection.Open()
        Dim Transaction As SqlTransaction = Connection.BeginTransaction
        Try

            Dim odaTablaMaestra As New daTablaMaestra

            For Each eMarcadorCotizacion As beMarcadorCotizacion In lista
                If eMarcadorCotizacion.IdMarcadorCotizacion = "0" Then
                    odaTablaMaestra.MarcadorCotizacionInsertar(Connection, eMarcadorCotizacion, Transaction)
                Else
                    odaTablaMaestra.MarcadorCotizacionActualizar(Connection, eMarcadorCotizacion, Transaction)
                End If
            Next
            Transaction.Commit()
            strReturn = "1"
        Catch ex As Exception
            Transaction.Rollback()
        End Try
        Return strReturn
    End Function
End Class