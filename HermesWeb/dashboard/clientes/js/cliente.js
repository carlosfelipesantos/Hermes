const usuario = {
  id: 1,
  nome: "Thales",
  tipo: "cliente"
};

const fretesRecentes = [
  {
    id: 1,
    origem: "São Paulo",
    destino: "Rio de Janeiro",
    status: "aceito",
    dataSolicitacao: "2026-03-08",
    peso: 120,
    valor: 450.00,
    podeExcluir: true
  },
  {
    id: 2,
    origem: "Campinas",
    destino: "Santos",
    status: "concluido",
    dataSolicitacao: "2026-03-05",
    peso: 80,
    valor: 280.00,
    podeExcluir: true
  },
  {
    id: 3,
    origem: "Sorocaba",
    destino: "Ribeirão Preto",
    status: "pendente",
    dataSolicitacao: "2026-03-09",
    peso: 50,
    valor: 190.00,
    podeExcluir: true
  }
];

let freteAtualId = null;

const nomeUsuario = document.getElementById("nomeUsuario");
const fretesGrid = document.getElementById("fretesGrid");
const feedbackContainer = document.getElementById("feedbackContainer");

nomeUsuario.textContent = usuario.nome;

function formatarValor(valor) {
  return valor.toLocaleString("pt-BR", {
    style: "currency",
    currency: "BRL"
  });
}

function getAcoesFrete(frete) {
  let acoes = "";

  if (frete.status === "aceito") {
    acoes += `
      <a class="btn btn-secondary btn-small" href="rastreamento.html?id=${frete.id}">
        <i class="fas fa-route"></i> Rastrear
      </a>
    `;
  } else if (frete.status === "concluido") {
    acoes += `
      <a class="btn btn-primary btn-small" href="avaliacao-frete.html?id=${frete.id}">
        <i class="fas fa-star"></i> Avaliar
      </a>
    `;
  }

  if (frete.podeExcluir) {
    acoes += `
      <button class="btn btn-danger btn-small btn-excluir-frete"
        data-frete-id="${frete.id}"
        data-frete-origem="${frete.origem}"
        data-frete-destino="${frete.destino}">
        <i class="fas fa-trash"></i> Excluir
      </button>
    `;
  }

  return acoes;
}

function renderFretes() {
  fretesGrid.innerHTML = "";

  if (!fretesRecentes.length) {
    fretesGrid.innerHTML = `<p class="empty-text">Nenhum frete encontrado.</p>`;
    return;
  }

  fretesRecentes.forEach(frete => {
    const card = document.createElement("div");
    card.className = "frete-card";
    card.id = `frete-${frete.id}`;

    card.innerHTML = `
      <div class="frete-info">
        <h3><i class="fas fa-map-marker-alt"></i> ${frete.origem} → ${frete.destino}</h3>
        <p><strong>Status:</strong> <span class="status ${frete.status.toLowerCase()}">${frete.status.toUpperCase()}</span></p>
        <p><strong>Data:</strong> ${frete.dataSolicitacao}</p>
        <p><strong>Peso:</strong> ${frete.peso} kg</p>
        <p><strong>Valor:</strong> ${formatarValor(frete.valor)}</p>
      </div>
      <div class="frete-actions">
        ${getAcoesFrete(frete)}
      </div>
    `;

    fretesGrid.appendChild(card);
  });

  bindEventosExcluir();
}

function bindEventosExcluir() {
  document.querySelectorAll(".btn-excluir-frete").forEach(btn => {
    btn.addEventListener("click", function () {
      freteAtualId = Number(this.dataset.freteId);
      document.getElementById("freteOrigem").textContent = this.dataset.freteOrigem;
      document.getElementById("freteDestino").textContent = this.dataset.freteDestino;
      document.getElementById("modalExcluirFrete").style.display = "block";
    });
  });
}

function fecharModalExclusao() {
  document.getElementById("modalExcluirFrete").style.display = "none";
  freteAtualId = null;
}

function confirmarExclusao() {
  if (!freteAtualId) return;

  const index = fretesRecentes.findIndex(f => f.id === freteAtualId);
  if (index !== -1) {
    const elemento = document.getElementById(`frete-${freteAtualId}`);
    if (elemento) {
      elemento.style.transition = "all 0.3s ease";
      elemento.style.opacity = "0";
      elemento.style.transform = "translateX(-100px)";
      setTimeout(() => {
        fretesRecentes.splice(index, 1);
        renderFretes();
        mostrarMensagem("Frete excluído com sucesso!", "success");
      }, 300);
    }
  }

  fecharModalExclusao();
}

function mostrarMensagem(mensagem, tipo) {
  feedbackContainer.innerHTML = `
    <div class="alert alert-${tipo}" style="background:${tipo === "success" ? "#d4edda" : "#f8d7da"}; color:${tipo === "success" ? "#155724" : "#721c24"}; padding:12px; border-radius:4px; margin:20px; border:1px solid ${tipo === "success" ? "#c3e6cb" : "#f5c6cb"};">
      <i class="fas ${tipo === "success" ? "fa-check-circle" : "fa-exclamation-triangle"}"></i>
      ${mensagem}
    </div>
  `;

  setTimeout(() => {
    feedbackContainer.innerHTML = "";
  }, 3000);
}

document.getElementById("closeModal").addEventListener("click", fecharModalExclusao);
document.getElementById("btnCancelarExclusao").addEventListener("click", fecharModalExclusao);
document.getElementById("btnConfirmarExclusao").addEventListener("click", confirmarExclusao);

window.addEventListener("click", function (event) {
  const modal = document.getElementById("modalExcluirFrete");
  if (event.target === modal) {
    fecharModalExclusao();
  }
});

renderFretes();