Public Class Estructuras

    Public Enum Accion
        Nuevo = 0
        Editar = 1
        Grabar = 2
        Cancelar = 3
        Eliminar = 4
        Consulta = 5
        Copiar = 6
        Aprobar = 7
        Solicitar = 8
        Aceptar = 9
    End Enum

    Public Class Entidad
        Public Const CartaPresentacion = "100"
        Public Const ResumenPropuesta = "101"
        Public Const AdicionalProducto = "102"
        Public Const AccesorioProducto = "103"
        Public Const CondicionesGeneralesPrime = "104"
        Public Const CondicionesGeneralesCSA = "105"
        Public Const CondicionesGeneralesAlquiler = "109"

        Public Const TerminosCondiciones = "106"
        Public Const DetalleProducto = "107"
        Public Const ProductoAlquiler = "108"

        Public Const Modelo = 10
        Public Const PlantillaCampo = 16
        Public Const AprobadorUsuario = 19
    End Class

    Public Enum EntidadTablaMaestra
        CondicionModelo = 0
        Estado = 1
        EstadoCotizacion = 2
        PlanMantenimiento = 3
        TipoAdicional = 4
        TipoModelo = 5
        TipoPersona = 6
        CategoriaTarifa = 7
        IGV = 8
        Mensaje = 9
        EstadoModelo = 10
        TipoSeccion = 11
    End Enum

    Public Enum EstadoCotizacion
        EnRegistro = 0
        EnAprobacion = 1
        Aprobado = 2
        Anulado = 3
        Rechazado = 4
        Ganada = 5
        Perdida = 6
        RechazadoObservado = 7
        Suspendida = 8
    End Enum

    Public Class EntidadMaestraDbs
        Public Const Corporacion As String = "COR"
        Public Const Compañia As String = "CIA"
        Public Const Moneda As String = "MON"
        Public Const Idioma As String = "IDI"
    End Class

    Public Class CategoriaTarifa
        Public Const UGM As String = "UGM"
        Public Const Construccion As String = "C"
    End Class

    Public Class Estado
        Public Const Habilitado As String = "1"
        Public Const Inahibilitado As String = "0"
    End Class

    Public Class TipoAdicional
        Public Const Entrenamiento As String = "E"
        Public Const Accesorio As String = "A"
    End Class

    Public Class TipoRegistroCotizacion
        Public Const Previa As String = "P"
        Public Const Normal As String = "N"
    End Class

    Public Class TipoDocumento
        Public Const Natural As String = "N"
        Public Const Juridica As String = "J"
    End Class

    Public Class TipoModelo
        Public Const Accesorio As String = "A"
        Public Const Maquina As String = "M"
        Public Const Motor As String = "O"
    End Class

    Public Class OrigenVisualizar
        Public Const Cotizacion As String = "C"
        Public Const CotizacionAdjunto As String = "A"
        Public Const Plantilla As String = "P"
    End Class

    Public Class IGV
        Public Const c_strIGV = "IGV"
    End Class

    Public Class MensajesAplicacion
        ''' <summary>
        ''' No se encuentra permitido el tipo de operación
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strGE001 As String = "GE001" '"No se encuentra permitido el tipo de operación"
        ''' <summary>
        ''' Registro actualizado
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strGE002 As String = "GE002" '"Registro actualizado"
        ''' <summary>
        ''' Registro eliminado
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strGE003 As String = "GE003" '"Registro eliminado"
        ''' <summary>
        ''' No se encontro el registro
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strGE004 As String = "GE004" '"No se encontro el registro"
        ''' <summary>
        ''' La solicitud de cotización ha sido rechazada
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strCO001 As String = "CO001" '"La solicitud de cotización ha sido rechazada"
        ''' <summary>
        ''' La solicitud de cotización ha sido aprobada
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strCO002 As String = "CO002" '"La solicitud de cotización ha sido aprobada"
        ''' <summary>
        ''' La cotización ha sido creada
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strCO003 As String = "CO003" '"La cotización ha sido creada"
        ''' <summary>
        ''' La solicitud ha sido enviada para su aprobación
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strCO004 As String = "CO004" '"La solicitud ha sido enviada para su aprobación"
        ''' <summary>
        ''' La solicitud requiere aprobación
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strCO005 As String = "CO005" '"La solicitud requiere aprobación"
        ''' <summary>
        ''' El usuario no se encuentra registrado como Aprobador
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strSE001 As String = "SE001" '"El usuario no se encuentra registrado como Aprobador"
        ''' <summary>
        ''' Verifique que el modelo este registrado
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strMO001 As String = "MO001" '"Verifique que el modelo este registrado"
        ''' <summary>
        ''' Verifique que el contacto este registrado
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strCC001 As String = "CC001" '"Verifique que el contacto este registrado"
        ''' <summary>
        ''' Verifique que el cliente este registrado
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strCL001 As String = "CL001" '"Verifique que el cliente este registrado"
        ''' <summary>
        ''' Plantilla de cotización no encontrada
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strPL001 As String = "PL001" '"Plantilla de cotización no encontrada"
        ''' <summary>
        ''' El documento no contiene ningun campo marcador
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strPL002 As String = "PL002" '"El documento no contiene ningun campo marcador"
        ''' <summary>
        ''' Sólo se permiten archivos de extenision .docx
        ''' </summary>
        ''' <remarks></remarks>
        Public Const c_strPL003 As String = "PL003" '"Sólo se permiten archivos de extenision .docx"
    End Class

    Public Class RolSeguridad
        Public Const Administrador As String = "00000002"
        Public Const Vendedor As String = "00000001"
        Public Const Aprobador As String = "00000003"
        Public Const Consultor As String = "00000004"
    End Class

    Public Class TipoProductoAlquiler
        'CodigoTipoArrendamiento(tabla ProductoAlquiler en BD)
        Public Const ALQUILER = "01"
        Public Const LEASING = "02"
    End Class
    Public Structure TipoProducto
        Public Shared PRIME As String = "Z001"
        Public Shared CSA As String = "Z002"
        Public Shared ACCESORIO As String = "Z003"
        Public Shared ALQUILER As String = "Z004"
        Public Const ALQUILER_LEASING As String = "Z004-02" ' 02:CodigoTipoArrendamiento(ProductoAlquiler) Solo para validaciones. siempre debe guardarse en BD como Z004
        Public Const SOLUCION_COMBINADA As String = "Z005"
    End Structure

    Public Structure CodigoSeccion
        Public Const CartaPresentacion As String = "001"
        Public Const PropuestaComercial As String = "002"
        Public Const CondicionesGenerales As String = "003"
        Public Const EspecificacionTecnica As String = "004"
        Public Const TerminosCondiciones As String = "005"
        Public Const PresentacionFSA As String = "006"
        Public Const PresentacionMercado As String = "007"
        Public Const RequisitosAprobacionFormalizacionCredito As String = "009"
        Public Const FormatoUCMIAnual As String = "010"
        Public Const FormatoUCMIEvento As String = "011"
    End Structure
    Public Structure TipoCotizacionCSA
        Public Shared PRIME = "P"
        Public Shared StandBy = "S"
        Public Shared Heaving = "H"
        Public Shared MONITOREO = "M"
    End Structure
    Public Structure UbicacionServidor
        Public Shared Desarrollo = "DEV" 'Ambiente desarrollo
        Public Shared Calidad = "QA" 'Ambiente Calidad
        Public Shared Produccion = "PRD" 'Ambiente Produccion
    End Structure

    Public Structure TablaHomologacion
        Public Const ROL_EDICION = "ROL_EDICION"
        Public Const GARANTIA = "GARANTIA"
        Public Const GLOSA_ORVISA = "GLOSA_ORVISA"
        Public Const ROL_BLOQUEO_VENDEDOR = "ROL_BLOQUEO_VENDEDOR"
        Public Const DURACION_PLAN_GENERADOR_PRIME = "DURACION_PLAN_GENERADOR_PRIME"
        Public Const DIR_COTIZACION_ALQUILER_PREVIA = "DIR_COTIZACION_ALQUILER_PREVIA"
        Public Const DIR_COTIZACION_ALQUILER_PREVIA_INFOADIC = "DIR_COTIZACION_ALQUILER_PREVIA_INFOADIC"
        Public Const DIR_COTIZACION_ALQUILER_LEASING = "DIR_COTIZACION_ALQUILER_LEASING"
        Public Const COD_PRODUCTO_CARACTERISTICA_PESO = "COD_PRODUCTO_CARACTERISTICA_PESO"
        Public Const COD_LINEA_COD_FAMILIA_SOLUCION_COMBINADA = "COD_LINEA_COD_FAMILIA_SOLUCION_COMBINADA"
        Public Const DIR_COTIZACION_ALQUILER_INFOADIC = "DIR_COTIZACION_ALQUILER_INFOADIC"
        Public Const INCLUYE_CUADRO_CONDICIONES_ESPECIFICAS_SC = "INCLUYE_CUADRO_CONDICIONES_ESPECIFICAS_SC"
        Public Const COD_TIPO_TELEFONO_RESPONSABLE = "COD_TIPO_TELEFONO_RESPONSABLE"
        Public Const CAMPOS_ADICIONALES_POR_LINEA = "CAMPOS_ADICIONALES_POR_LINEA"
        Public Const INCLUYE_CUADRO_VIGENCIA_SC = "INCLUYE_CUADRO_VIGENCIA_SC"
        Public Const PLAZO_ENTREGA_ESTIMADO = "PLAZO_ENTREGA_ESTIMADO"
        Public Const REG_COTIZACION_LISTA_COMPLETA_EMPRESA = "REG_COTIZACION_LISTA_COMPLETA_EMPRESA"
        Public Const REG_COTIZACION_LISTA_COMPLETA_TIPO_PRODUCTO = "REG_COTIZACION_LISTA_COMPLETA_TIPO_PRODUCTO"
        Public Const REG_COTIZACION_LISTA_COMPLETA_ACCESORIO = "REG_COTIZACION_LISTA_COMPLETA_ACCESORIO"
        '14/11
        Public Const COD_CLCVARIABLE_INICIAL = "COD_CLCVARIABLE_INICIAL"
        Public Const COD_CLCVARIABLE_IGV = "COD_CLCVARIABLE_IGV"
        Public Const COD_CLCVARIABLE_FINANCE_FREE = "COD_CLCVARIABLE_FINANCE_FREE"
        Public Const COD_CLCVARIABLE_PLAZOMESES1 = "COD_CLCVARIABLE_PLAZOMESES1"
        Public Const COD_CLCVARIABLE_PLAZOMESES2 = "COD_CLCVARIABLE_PLAZOMESES2"
        Public Const COD_CLCVARIABLE_TASA_INTERES1 = "COD_CLCVARIABLE_TASA_INTERES1"
        Public Const COD_CLCVARIABLE_TASA_INTERES2 = "COD_CLCVARIABLE_TASA_INTERES2"
    End Structure
End Class