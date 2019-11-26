using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Seguridad
{
    public class EntegasBL
    {

        Contexto _contexto;
        public BindingList<Entrega> ListaEntregas { get; set; }

        public EntegasBL()
        {
            _contexto = new Contexto();
        }

        public BindingList<Entrega> ObtenerEntrega()
        {
            _contexto.Entregas.Include("Detalles").Load();
            return ListaEntregas;
        }

        public void CancelarCambios()
        {
            foreach (var item in _contexto.ChangeTracker.Entries())
            {
                item.State = EntityState.Unchanged;
                item.Reload();
            }
        }


        public Resultado GuardarEntrega(Entrega entrega)
        {
            var resultado = Validar(entrega);
            if (resultado.Exitoso == false)
            {
                return resultado;
            }

            _contexto.SaveChanges();
            resultado.Exitoso = true;
            return resultado;
        }

        public void AgregarEntrega()
        {
            var nuevaentrega = new Entrega();
            ListaEntregas.Add(nuevaentrega);

        }

        public void AgregarDetalles(Entrega entrega)
        {
            if (entrega != null)
            {
                var nuevoDetalle = new Detalles();
                entrega.DetallesEntrega.Add(nuevoDetalle);
            }
        }

        public void RemoverDetalles(Entrega entrega, Detalles detalle)
        {
            if (entrega != null && detalle != null)
            {
                entrega.DetallesEntrega.Remove(detalle);


            }

        }
      




        private Resultado Validar(Entrega entrega)
        {
            var resultado = new Resultado();
            resultado.Exitoso = true;
            if (entrega == null)
            {
                resultado.Mensaje = "Agregue una factura para poder salvarla";
                resultado.Exitoso = false;
                return resultado;

            }
            if (entrega.Activo ==false)
            {
                resultado.Mensaje = "Factura anulada, no se puede editar";
                resultado.Exitoso = false;
            }

            return resultado;
        }

        public bool AnularEntrega(int id)
        {
            foreach (var entrega in ListaEntregas)
            {
                if (entrega.Id== id)
                {
                    entrega.Activo = false;
                    _contexto.SaveChanges();
                    return true;

                }

            }
            return false;
        }
    }


    public class Entrega
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string InformeRevison { get; set; }
        public double Subtotal { get; set; }
        public double Impuesto { get; set; }
        public double Total { get; set; }
        public BindingList<Detalles> DetallesEntrega { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }
        public int TipoId { get; set; }
        public Tipo Tipo { get; set; }
        public bool Activo { get; set; }

        public Entrega()
        {
            Fecha = DateTime.Now;
            DetallesEntrega = new BindingList<Detalles>();
            Activo = true;
           // Subtotal= preciotextbox.text;
            Impuesto = Subtotal * 0.15;
            Total = Subtotal + Impuesto;
        }
    }
    public class Detalles
    {


        public int Id { get; set; }
        public string Detalle { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }

    }


}
