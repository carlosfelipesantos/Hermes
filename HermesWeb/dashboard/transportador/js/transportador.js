const usuario = {
  id: 2,
  nome: "Transportador Hermes",
  tipo: "transportador"
};

const fretesDisponiveis = [{ id: 10 }, { id: 11 }, { id: 12 }];

const fretesAceitos = [
  {
    id: 1,
    origem: "São Paulo",
    destino: "Campinas",
    peso: 150,
    descricaoCarga: "Eletrônicos",
    valor: 600,
    idCliente: 88,
    dataSolicitacao: "2026-03-08",
    tipo: "ACEITO"
  }
];

const fretesEmAndamento = [
  {
    id: 2,
    origem: "Santos",
    destino: "Sorocaba",
    peso: 200,
    descricaoCarga: "Móveis",
    valor: 750,
    idCliente: 25,
    dataSolicitacao: "2026-03-07",
    tipo: "EM_ANDAMENTO"
  }
];

const fretesConcluidos = [
  {
    id: 3,
    origem: "Ribeirão Preto",
    destino: "Franca",
    peso: 90,
    valor: 320,
    idCliente: 14,
    dataConclusao: "2026-03-06",
    tipo: "CONCLUIDO"
  }
];

const veiculos = [
  {
    id: 1,
    marca: "Mercedes",
    modelo: "Atego",
    placa: "ABC-1234",
    tipoVeiculo: "Caminhão",
    ano: 2020,
    capacidade: 5000,
    cor: "Branco",
    dataCadastro: "2026-02-20"
  }
];

let freteAtualId = null;
let freteAtualTipo = null;

document.getElementById("nomeTransportador").textContent = usuario.nome;

function formatarValor(valor) {
  return valor.toLocaleString("pt-BR", {
    style: "currency",
    currency: "BRL"
  });
}

function atualizarStats() {
  document.getElementById("statDisponiveis").textContent = fretesDisponiveis.length;
  document.getElementById("statAceitos").textContent = fretesAceitos.length;
  document.getElementById("statAndamento").textContent = fretesEmAndamento.length;
  document.getElementById("statConcluidos").textContent = fretesConcluidos.length;
}

function renderLista(lista, containerId, statusTexto) {
  const container = document.getElementById(containerId);
  container.innerHTML = "";

  if (!lista.length) {
    container.innerHTML = `
      <div class="empty-state">
        <i class="fas fa-box-open fa-3x"></i>
        <h3>Nenhum frete encontrado</h3>
        <p>Não há itens nesta seção no momento.</p>
      </div>
    `;
    return;
  }

  lista.forEach(frete => {
    const card = document.createElement("div");
    card.className = "frete-card";
    card.id = `frete-${frete.id}`;

    card.innerHTML = `
      <div class="frete-header">
        <h3><i class="fas fa-map-marker-alt"></i> ${frete.origem} → ${frete.destino}</h3>
        <span class="frete-status ${frete.tipo.toLowerCase()}">${statusTexto}</span>
      </div>

      <div class="frete-info">
        <p><strong>Peso:</strong> ${frete.peso} kg</p>
        ${frete.descricaoCarga ? `<p><strong>Descrição:</strong> ${frete.descricaoCarga}</p>` : ""}
        <p><strong>Valor:</strong> ${formatarValor(frete.valor)}</p>
        <p><strong>Cliente:</strong> ID ${frete.idCliente}</p>
        ${frete.dataSolicitacao ? `<p><strong>Data:</strong> ${frete.dataSolicitacao}</p>` : ""}
        ${frete.dataConclusao ? `<p><strong>Data Conclusão:</strong> ${frete.dataConclusao}</p>` : ""}
      </div>

      <div class="frete-actions">
        ${frete.tipo === "ACEITO" ? `
          <button class="btn btn-primary btn-small btn-iniciar" data-id="${frete.id}">
            <i class="fas fa-play"></i> Iniciar Frete
          </button>
        ` : ""}

        ${frete.tipo === "EM_ANDAMENTO" ? `
          <button class="btn btn-success btn-small btn-finalizar" data-id="${frete.id}">
            <i class="fas fa-check-double"></i> Finalizar Frete
          </button>
        ` : ""}

        ${frete.tipo === "CONCLUIDO" ? `
          <span style="color:#27ae60;font-weight:600;">
            <i class="fas fa-check-circle"></i> Frete finalizado com sucesso
          </span>
        ` : ""}

        <button class="btn btn-danger btn-small btn-excluir-frete"
          data-frete-id="${frete.id}"
          data-frete-origem="${frete.origem}"
          data-frete-destino="${frete.destino}"
          data-frete-tipo="${frete.tipo}">
          <i class="fas fa-trash"></i> Excluir
        </button>
      </div>
    `;

    container.appendChild(card);
  });
}

function renderVeiculos() {
  const container = document.getElementById("veiculosGrid");
  container.innerHTML = "";

  if (!veiculos.length) {
    container.innerHTML = `
      <div class="empty-state">
        <i class="fas fa-truck fa-3x"></i>
        <h3>Nenhum veículo cadastrado</h3>
        <p>Cadastre seu primeiro veículo para começar a aceitar fretes.</p>
        <a href="cadastrar-veiculo.html" class="btn btn-primary" style="margin-top: 1rem;">
          Cadastrar Veículo
        </a>
      </div>
    `;
    return;
  }

  veiculos.forEach(v => {
    const card = document.createElement("div");
    card.className = "frete-card";
    card.style.borderLeft = "4px solid #2ecc71";

    card.innerHTML = `
      <div class="frete-header">
        <h3><i class="fas fa-truck"></i> ${v.marca} ${v.modelo}</h3>
        <span class="frete-status concluido">${v.placa}</span>
      </div>

      <div class="frete-info">
        <p><strong>Tipo:</strong> ${v.tipoVeiculo}</p>
        <p><strong>Ano:</strong> ${v.ano}</p>
        <p><strong>Capacidade:</strong> ${v.capacidade} kg</p>
        <p><strong>Cor:</strong> ${v.cor || "Não informada"}</p>
        <p><strong>Cadastrado em:</strong> ${v.dataCadastro}</p>
      </div>

      <div class="frete-actions">
        <button class="btn btn-danger btn-small btn-excluir-veiculo" data-veiculo-id="${v.id}">
          <i class="fas fa-trash"></i> Excluir
        </button>
      </div>
    `;

    container.appendChild(card);
  });
}

function abrirModalExclusao(id, origem, destino, tipo) {
  freteAtualId = Number(id);
  freteAtualTipo = tipo;
  document.getElementById("freteOrigem").textContent = origem;
  document.getElementById("freteDestino").textContent = destino;
  document.getElementById("modalExcluirFrete").style.display = "block";
}

function fecharModalExclusao() {
  document.getElementById("modalExcluirFrete").style.display = "none";
  freteAtualId = null;
  freteAtualTipo = null;
}

function confirmarExclusao() {
  if (!freteAtualId || !freteAtualTipo) return;

  const mapas = {
    ACEITO: fretesAceitos,
    EM_ANDAMENTO: fretesEmAndamento,
    CONCLUIDO: fretesConcluidos
  };

  const lista = mapas[freteAtualTipo];
  if (!lista) return;

  const index = lista.findIndex(item => item.id === freteAtualId);
  if (index !== -1) {
    lista.splice(index, 1);
  }

  renderTudo();
  fecharModalExclusao();
  mostrarMensagem("Frete excluído com sucesso!", "success");
}

function bindEventos() {
  document.querySelectorAll(".btn-excluir-frete").forEach(btn => {
    btn.addEventListener("click", function () {
      abrirModalExclusao(
        this.dataset.freteId,
        this.dataset.freteOrigem,
        this.dataset.freteDestino,
        this.dataset.freteTipo
      );
    });
  });

  document.querySelectorAll(".btn-iniciar").forEach(btn => {
    btn.addEventListener("click", function () {
      const id = Number(this.dataset.id);
      const index = fretesAceitos.findIndex(f => f.id === id);
      if (index !== -1) {
        const frete = fretesAceitos.splice(index, 1)[0];
        frete.tipo = "EM_ANDAMENTO";
        fretesEmAndamento.push(frete);
        renderTudo();
        mostrarMensagem("Frete iniciado com sucesso!", "success");
      }
    });
  });

  document.querySelectorAll(".btn-finalizar").forEach(btn => {
    btn.addEventListener("click", function () {
      const id = Number(this.dataset.id);
      const index = fretesEmAndamento.findIndex(f => f.id === id);
      if (index !== -1) {
        const frete = fretesEmAndamento.splice(index, 1)[0];
        frete.tipo = "CONCLUIDO";
        frete.dataConclusao = "2026-03-10";
        delete frete.descricaoCarga;
        fretesConcluidos.push(frete);
        renderTudo();
        mostrarMensagem("Frete finalizado com sucesso!", "success");
      }
    });
  });

  document.querySelectorAll(".btn-excluir-veiculo").forEach(btn => {
    btn.addEventListener("click", function () {
      const id = Number(this.dataset.veiculoId);
      const index = veiculos.findIndex(v => v.id === id);
      if (index !== -1) {
        veiculos.splice(index, 1);
        renderTudo();
        mostrarMensagem("Veículo excluído com sucesso!", "success");
      }
    });
  });
}

function mostrarMensagem(mensagem, tipo) {
  document.querySelectorAll(".alert-floating").forEach(el => el.remove());

  const alerta = document.createElement("div");
  alerta.className = `alert-floating alert-${tipo}`;
  alerta.innerHTML = `
    <i class="fas ${tipo === "success" ? "fa-check-circle" : "fa-exclamation-triangle"}"></i>
    ${mensagem}
  `;

  document.body.appendChild(alerta);

  setTimeout(() => {
    alerta.remove();
  }, 3000);
}

function renderTudo() {
  atualizarStats();
  renderLista(fretesAceitos, "fretesAceitosGrid", "ACEITO");
  renderLista(fretesEmAndamento, "fretesAndamentoGrid", "EM ANDAMENTO");
  renderLista(fretesConcluidos, "fretesConcluidosGrid", "CONCLUÍDO");
  renderVeiculos();
  bindEventos();
}

document.getElementById("closeModal").addEventListener("click", fecharModalExclusao);
document.getElementById("btnCancelarExclusao").addEventListener("click", fecharModalExclusao);
document.getElementById("btnConfirmarExclusao").addEventListener("click", confirmarExclusao);

window.addEventListener("click", function (event) {
  const modal = document.getElementById("modalExcluirFrete");
  if (event.target === modal) fecharModalExclusao();
});

renderTudo();