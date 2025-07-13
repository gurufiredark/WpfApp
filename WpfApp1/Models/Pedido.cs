using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataDaVenda { get; set; }

        public Pessoa Pessoa { get; set; }

        public List<ItemPedido> Produtos { get; set; }

        public FormaDePagamento FormaDePagamento { get; set; }

        public StatusPedido Status { get; set; }
        
        public decimal ValorTotal {
            get
            {
                return Produtos?.Sum(item => item.Produto.Valor * item.Quantidade) ?? 0;
            }
        }
    }
}
