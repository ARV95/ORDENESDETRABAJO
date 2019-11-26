using BL.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARYA
{
    public partial class Entregas : Form
    {

        TiposBL _tiposBL; 
        ClientesBL _clientesBL;
        EquiposBL _equiposBL;
        EntegasBL _entregasBL;

        public Entregas()
        {
            InitializeComponent();


            _tiposBL = new TiposBL();
            listaTiposBindingSource.DataSource = _tiposBL.ObtenerTipos();

            _clientesBL = new ClientesBL();
            listaClientesBindingSource.DataSource = _clientesBL.ObtenerClientes();

            _equiposBL = new EquiposBL();
            listaEquiposBindingSource.DataSource = _equiposBL.ObtenerEquipos();

            _entregasBL = new EntegasBL();
            listaEntregasBindingSource.DataSource = _entregasBL.ObtenerEntrega();


        }

        private void listaEntregasDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Entregas_Load(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            _entregasBL.AgregarEntrega();
            listaEntregasBindingSource.MoveLast();
            
            DeshabilitarHabilitarBotones(false);
           
        }
        private void DeshabilitarHabilitarBotones(bool v)
        {
            bindingNavigatorMoveFirstItem.Enabled = v;
            bindingNavigatorMoveLastItem.Enabled = v;
            bindingNavigatorMovePreviousItem.Enabled = v;
            bindingNavigatorMoveNextItem.Enabled = v;
            bindingNavigatorPositionItem.Enabled = v;
            bindingNavigatorAddNewItem.Enabled = v;
            bindingNavigatorDeleteItem.Enabled = v;

            toolStripButton1.Visible = !v;
        }

        private void listaEntregasBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            listaEntregasBindingSource.EndEdit();

            var entrega = (Entrega)listaEntregasBindingSource.Current;
            var resultado = _entregasBL.GuardarEntrega(entrega);

            if (resultado.Exitoso== true)
            {

                listaEntregasBindingSource.ResetBindings(false);
                DeshabilitarHabilitarBotones(true);
                MessageBox.Show("Guardado");
            }
            else
            {
                MessageBox.Show(resultado.Mensaje);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DeshabilitarHabilitarBotones(true);
            _entregasBL.CancelarCambios();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var entrega = (Entrega)listaEntregasBindingSource.Current;
            _entregasBL.AgregarDetalles(entrega);
            DeshabilitarHabilitarBotones(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var entrega = (Entrega)listaEntregasBindingSource.Current;
            var entregaDetalle = (Detalles)detallesEntregaBindingSource.Current;
            _entregasBL.RemoverDetalles(entrega, entregaDetalle);
            DeshabilitarHabilitarBotones(false);
        }

        private void detallesEntregaDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;

        }

        private void detallesEntregaDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var entrega = (Entrega)listaEntregasBindingSource.Current;
            _entregasBL.AgregarDetalles(entrega);
            DeshabilitarHabilitarBotones(false);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var entrega = (Entrega)listaEntregasBindingSource.Current;
            var detalle = (Detalles)detallesEntregaBindingSource.Current;

            _entregasBL.RemoverDetalles(entrega, detalle);

            DeshabilitarHabilitarBotones(false);

        }

        private void detallesDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {

            if (idTextBox.Text != "")
            {
                var resultado = MessageBox.Show("Desea anular esta factura?", "Anular", MessageBoxButtons.YesNo);
                if (resultado== DialogResult.Yes)
                {
                    var id = Convert.ToInt32(idTextBox.Text);
                    Anular(id);

                }
            }
        }

        private void Anular(int id)
        {

            var resultado = _entregasBL.AnularEntrega(id);
            if (resultado == true)
            {
                listaEntregasBindingSource.ResetBindings(false);
            }
            else
            {
                MessageBox.Show("Ocurrio un error al anular la factura");
            }
        }
    }
}
