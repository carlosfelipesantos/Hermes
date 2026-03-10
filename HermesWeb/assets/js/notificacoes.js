const notificacoes = [
  {
    id: 1,
    titulo: "Frete aceito",
    mensagem: "Seu frete foi aceito por um transportador.",
    tipo: "frete_aceito",
    lida: false,
    dataCriacao: "2026-03-09T14:30:00",
    idFrete: 101
  },
  {
    id: 2,
    titulo: "Frete em andamento",
    mensagem: "O frete está em rota de entrega.",
    tipo: "frete_em_andamento",
    lida: false,
    dataCriacao: "2026-03-09T10:15:00",
    idFrete: 102
  },
  {
    id: 3,
    titulo: "Avaliação recebida",
    mensagem: "Você recebeu uma nova avaliação.",
    tipo: "avaliacao_recebida",
    lida: true,
    dataCriacao: "2026-03-08T18:45:00",
    idFrete: 0
  }
];

const notificacoesList = document.getElementById("notificacoesList");
const naoLidasBadge = document.getElementById("naoLidasBadge");
const actionsContainer = document.getElementById("actionsContainer");
const btnMarcarTodas = document.getElementById("btnMarcarTodas");

function getIcon(tipo) {
  switch (tipo) {
    case "frete_aceito":
      return "fas fa-truck-loading";
    case "frete_concluido":
      return "fas fa-check-circle";
    case "avaliacao_recebida":
      return "fas fa-star";
    case "novo_frete_disponivel":
      return "fas fa-box";
    case "frete_em_andamento":
      return "fas fa-truck-moving";
    default:
      return "fas fa-bell";
  }
}

function formatarData(dataString) {
  if (!dataString) return "";

  const data = new Date(dataString);

  const dataFormatada = data.toLocaleDateString("pt-BR");
  const horaFormatada = data.toLocaleTimeString("pt-BR", {
    hour: "2-digit",
    minute: "2-digit"
  });

  return `${dataFormatada} às ${horaFormatada}`;
}

function contarNaoLidas() {
  return notificacoes.filter(notif => !notif.lida).length;
}

function renderHeader() {
  const naoLidas = contarNaoLidas();

  if (naoLidas > 0) {
    naoLidasBadge.style.display = "inline-block";
    naoLidasBadge.textContent = `${naoLidas} não lida(s)`;

    actionsContainer.style.display = "block";
  } else {
    naoLidasBadge.style.display = "none";
    actionsContainer.style.display = "none";
  }
}

function renderNotificacoes() {
  notificacoesList.innerHTML = "";

  if (notificacoes.length === 0) {
    notificacoesList.innerHTML = `
      <div class="empty-state">
        <i class="fas fa-bell-slash"></i>
        <h3>Nenhuma notificação</h3>
        <p>Você não tem notificações no momento.</p>
      </div>
    `;
    return;
  }

  notificacoes.forEach((notif, index) => {
    const item = document.createElement("div");
    item.className = `notificacao-item ${notif.lida ? "" : "nao-lida"}`;

    item.innerHTML = `
      <div class="notificacao-icon">
        <i class="${getIcon(notif.tipo)}"></i>
      </div>

      <div class="notificacao-content">
        <h4>${notif.titulo || "Notificação"}</h4>
        <p>${notif.mensagem || ""}</p>
        <small>${formatarData(notif.dataCriacao)}</small>
      </div>

      <div class="notificacao-actions">
        ${!notif.lida ? `
          <button class="btn-marcar-lida" data-id="${notif.id}" title="Marcar como lida">
            <i class="fas fa-check"></i>
          </button>
        ` : ""}

        ${notif.idFrete > 0 ? `
          <a href="fretes/rastreamento.html?id=${notif.idFrete}" class="btn-ver-frete">
            Ver Frete
          </a>
        ` : ""}
      </div>
    `;

    item.style.opacity = "0";
    item.style.transform = "translateY(20px)";

    notificacoesList.appendChild(item);

    setTimeout(() => {
      item.style.transition = "all 0.5s ease-out";
      item.style.opacity = "1";
      item.style.transform = "translateY(0)";
    }, index * 100);
  });

  adicionarEventos();
}

function adicionarEventos() {
  const botoesMarcarLida = document.querySelectorAll(".btn-marcar-lida");

  botoesMarcarLida.forEach(botao => {
    botao.addEventListener("click", function () {
      const id = Number(this.dataset.id);
      marcarComoLida(id);
    });
  });
}

function marcarComoLida(id) {
  const notificacao = notificacoes.find(notif => notif.id === id);

  if (notificacao) {
    notificacao.lida = true;
    renderHeader();
    renderNotificacoes();
  }
}

function marcarTodasComoLidas() {
  const confirmar = confirm("Tem certeza que deseja marcar todas as notificações como lidas?");

  if (!confirmar) return;

  notificacoes.forEach(notif => {
    notif.lida = true;
  });

  renderHeader();
  renderNotificacoes();
}

btnMarcarTodas?.addEventListener("click", marcarTodasComoLidas);

renderHeader();
renderNotificacoes();