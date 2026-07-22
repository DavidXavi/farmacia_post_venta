import { useEffect, useState } from 'react'
import { api } from '../api/client'
import { useAuth } from '../auth/AuthContext'
import { AyudaFormulario } from '../components/AyudaFormulario'

const TIPOS_BENEFICIO = ['DescuentoPorcentaje', 'DescuentoMonto', 'LlevaNPagaM']

const AYUDA_PROMOCIONES = [
  "Elige el tipo de beneficio: Descuento por porcentaje, Descuento por monto fijo, o 'Lleva N Paga M' (unidades gratis según cantidad).",
  'La cantidad mínima define desde cuántas unidades de un producto participante se activa la promoción.',
  "Marca 'Requiere cliente identificado' solo si la promoción exige tener un cliente asociado a la venta.",
  'Selecciona los productos participantes: la promoción no se aplica a productos fuera de esta lista.',
  'Una venta nunca aplica la misma promoción dos veces, ni más de una promoción por línea.',
]

export function PromocionesPage() {
  const { session } = useAuth()
  const [promociones, setPromociones] = useState([])
  const [productos, setProductos] = useState([])
  const [mensaje, setMensaje] = useState(null)
  const [form, setForm] = useState({
    nombre: '',
    descripcion: '',
    tipoBeneficio: 'DescuentoPorcentaje',
    valorBeneficio: '',
    requiereCliente: false,
    cantidadMinima: 1,
    fechaInicio: '',
    fechaFin: '',
    productosParticipantes: [],
  })

  function cargarPromociones() {
    api.get('/api/promociones').then(setPromociones).catch((e) => setMensaje(e.message))
  }

  useEffect(() => {
    cargarPromociones()
    api.get('/api/productos').then(setProductos)
  }, [])

  function actualizarCampo(campo, valor) {
    setForm((prev) => ({ ...prev, [campo]: valor }))
  }

  function alternarProducto(productoId) {
    setForm((prev) => ({
      ...prev,
      productosParticipantes: prev.productosParticipantes.includes(productoId)
        ? prev.productosParticipantes.filter((id) => id !== productoId)
        : [...prev.productosParticipantes, productoId],
    }))
  }

  async function registrar(e) {
    e.preventDefault()
    setMensaje(null)
    try {
      await api.post('/api/promociones', {
        ...form,
        valorBeneficio: Number(form.valorBeneficio),
        cantidadMinima: Number(form.cantidadMinima),
        fechaInicio: form.fechaInicio || null,
        fechaFin: form.fechaFin || null,
      })
      setMensaje('Promocion registrada.')
      cargarPromociones()
    } catch (err) {
      setMensaje(err.message)
    }
  }

  async function desactivar(id) {
    try {
      await api.patch(`/api/promociones/${id}/desactivar?usuarioId=${session.usuarioId}`)
      cargarPromociones()
    } catch (err) {
      setMensaje(err.message)
    }
  }

  return (
    <section>
      <h1>
        Promociones
        <AyudaFormulario titulo="Cómo registrar una promoción" pasos={AYUDA_PROMOCIONES} />
      </h1>

      <form className="tarjeta" onSubmit={registrar}>
        <h3>Nueva promocion</h3>
        <label>
          Nombre
          <input value={form.nombre} onChange={(e) => actualizarCampo('nombre', e.target.value)} required />
        </label>
        <label>
          Descripcion
          <input value={form.descripcion} onChange={(e) => actualizarCampo('descripcion', e.target.value)} />
        </label>
        <label>
          Tipo de beneficio
          <select value={form.tipoBeneficio} onChange={(e) => actualizarCampo('tipoBeneficio', e.target.value)}>
            {TIPOS_BENEFICIO.map((t) => <option key={t} value={t}>{t}</option>)}
          </select>
        </label>
        <label>
          Valor del beneficio
          <input type="number" step="0.01" value={form.valorBeneficio} onChange={(e) => actualizarCampo('valorBeneficio', e.target.value)} required />
        </label>
        <label>
          Cantidad minima
          <input type="number" min="1" value={form.cantidadMinima} onChange={(e) => actualizarCampo('cantidadMinima', e.target.value)} required />
        </label>
        <label>
          Vigencia inicio
          <input type="date" value={form.fechaInicio} onChange={(e) => actualizarCampo('fechaInicio', e.target.value)} />
        </label>
        <label>
          Vigencia fin
          <input type="date" value={form.fechaFin} onChange={(e) => actualizarCampo('fechaFin', e.target.value)} />
        </label>
        <label className="checkbox">
          <input type="checkbox" checked={form.requiereCliente} onChange={(e) => actualizarCampo('requiereCliente', e.target.checked)} />
          Requiere cliente identificado
        </label>
        <label>Productos participantes</label>
        <div className="resultado">
          {productos.map((p) => (
            <label key={p.id} className="checkbox">
              <input type="checkbox" checked={form.productosParticipantes.includes(p.id)} onChange={() => alternarProducto(p.id)} />
              {p.nombreComercial}
            </label>
          ))}
        </div>
        <button type="submit">Registrar promocion</button>
      </form>

      {mensaje && <p className="aviso">{mensaje}</p>}

      <div className="tarjeta">
        <table>
          <thead>
            <tr><th>Nombre</th><th>Tipo</th><th>Valor</th><th>Vigencia</th><th>Activa</th><th></th></tr>
          </thead>
          <tbody>
            {promociones.map((p) => (
              <tr key={p.id}>
                <td>{p.nombre}</td>
                <td>{p.tipoBeneficio}</td>
                <td>{p.valorBeneficio}</td>
                <td>{p.fechaInicio ?? '-'} a {p.fechaFin ?? '-'}</td>
                <td>{p.activa ? 'Si' : 'No'}</td>
                <td>{p.activa && <button onClick={() => desactivar(p.id)}>Desactivar</button>}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </section>
  )
}
