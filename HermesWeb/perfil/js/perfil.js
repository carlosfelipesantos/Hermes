const perfil = {
  usuario: {
    nome: "Thales Marconsini",
    email: "thales@email.com",
    telefone: "(17) 99999-9999",
    estado: "SP",
    cidade: "Santa Fé do Sul",
    endereco: "Rua Exemplo, 123",
    documento: "123.456.789-00",
    veiculo: "Caminhão 3/4",
    ddd: "17",
    tipoUsuario: "transportador"
  },

  historicoFretes: [
    {
      origem: "São Paulo",
      destino: "Campinas",
      status: "Concluído",
      valor: 450.00
    },
    {
      origem: "Jales",
      destino: "Fernandópolis",
      status: "Em andamento",
      valor: 180.00
    }
  ],

  avaliacoes: [
    {
      idFrete: 101,
      nota: 5,
      comentario: "Entrega rápida e muito cuidadosa.",
      dataAvaliacao: "2026-03-05",
      foto: "assets/images/exemplo-entrega.jpg"
    },
    {
      idFrete: 102,
      nota: 4,
      comentario: "Bom atendimento e pontualidade.",
      dataAvaliacao: "2026-03-07",
      foto: ""
    }
  ],

  avaliacoesFeitas: [
    {
      nota: 5,
      comentario: "Transportador excelente, recomendo.",
      dataAvaliacao: "2026-03-02",
      foto: ""
    }
  ]
};

const usuario = perfil.usuario;
const tipoUsuario = usuario.tipoUsuario;
const mediaAvaliacoes = perfil.avaliacoes.length
  ? perfil.avaliacoes.reduce((acc, av) => acc + av.nota, 0) / perfil.avaliacoes.length
  : 0;

function inicialDoNome(nome) {
  return nome?.trim()?.charAt(0)?.toUpperCase() || "U";
}

function estrelasHTML(nota, classe = "") {
  let html = `<div class="${classe}">`;
  for (let i = 1; i <= 5; i++) {
    html += `<i class="fas fa-star ${i <= nota ? "active" : ""}"></i>`;
  }
  html += `</div>`;
  return html;
}

function mostrarAlertaSucesso(mensagem) {
  const alertContainer = document.getElementById("alertContainer");
  alertContainer.innerHTML = `
    <div class="alert-overlay">
      <div class="alert-box success">
        <i class="fa-solid fa-check-circle"></i>
        <span>${mensagem}</span>
      </div>
    </div>
  `;

  setTimeout(() => {
    alertContainer.innerHTML = "";
  }, 3000);
}

function preencherPerfil() {
  document.getElementById("perfilInicial").textContent = inicialDoNome(usuario.nome);
  document.getElementById("perfilNome").textContent = usuario.nome || "";
  document.getElementById("perfilEmail").textContent = usuario.email || "";
  document.getElementById("perfilTelefone").textContent = usuario.telefone || "";
  document.getElementById("perfilTipoTag").textContent =
    tipoUsuario.toLowerCase() === "transportador" ? "Transportador" : "Cliente";

  document.getElementById("perfilDescricao").textContent =
    tipoUsuario.toLowerCase() === "transportador"
      ? "Gerencie seus dados, veículo e acompanhe seus fretes e avaliações."
      : "Atualize suas informações e acompanhe o histórico de fretes e avaliações.";

  document.getElementById("perfilLocalizacao").textContent =
    !usuario.cidade && !usuario.estado
      ? "Localização não informada"
      : `${usuario.cidade || ""}${usuario.cidade && usuario.estado ? " - " : ""}${usuario.estado || ""}`;

  document.getElementById("perfilQtdFretes").textContent = perfil.historicoFretes.length;
  document.getElementById("perfilFretesLabel").textContent =
    tipoUsuario.toLowerCase() === "transportador" ? "Fretes realizados" : "Fretes solicitados";
  document.getElementById("perfilDdd").textContent = `DDD ${usuario.ddd || "--"}`;

  document.getElementById("nome").value = usuario.nome || "";
  document.getElementById("email").value = usuario.email || "";
  document.getElementById("telefone").value = usuario.telefone || "";
  document.getElementById("estado").value = usuario.estado || "";
  document.getElementById("cidade").value = usuario.cidade || "";
  document.getElementById("endereco").value = usuario.endereco || "";
  document.getElementById("documento").value = usuario.documento || "";
  document.getElementById("ddd").value = usuario.ddd || "";

  if (tipoUsuario.toLowerCase() === "transportador") {
    document.getElementById("veiculoGroup").style.display = "block";
    document.getElementById("veiculo").value = usuario.veiculo || "";

    if (mediaAvaliacoes > 0) {
      document.getElementById("perfilRatingBox").innerHTML = `
        <div class="perfil-rating">
          ${estrelasHTML(Math.round(mediaAvaliacoes), "rating-stars")}
          <span class="rating-text">${mediaAvaliacoes.toFixed(1)} (${perfil.avaliacoes.length} avaliações)</span>
        </div>
      `;
    }
  }
}

function renderHistorico() {
  const container = document.getElementById("historicoContainer");

  if (!perfil.historicoFretes.length) {
    container.innerHTML = `
      <div class="empty-state">
        <i class="fa-regular fa-box-archive"></i>
        <p>Nenhum frete encontrado no histórico.</p>
      </div>
    `;
    return;
  }

  container.innerHTML = `
    <div class="perfil-history-list">
      ${perfil.historicoFretes.map(f => `
        <div class="perfil-history-item">
          <div class="history-main">
            <div class="history-route">
              <span class="history-label">Origem</span>
              <p>${f.origem}</p>
            </div>
            <div class="history-route">
              <span class="history-label">Destino</span>
              <p>${f.destino}</p>
            </div>
          </div>

          <div class="history-meta">
            <span class="history-status history-status-${(f.status || "").toLowerCase().replace(/\s+/g, "-")}">
              ${f.status || "Sem status"}
            </span>
            <span class="history-value">
              <i class="fa-solid fa-coins"></i>
              R$ ${Number(f.valor).toFixed(2)}
            </span>
          </div>
        </div>
      `).join("")}
    </div>
  `;
}

function renderAvaliacoesRecebidas() {
  if (tipoUsuario.toLowerCase() !== "transportador") return;

  document.getElementById("avaliacoesRecebidasCard").style.display = "block";

  const resumo = document.getElementById("avaliacoesResumo");
  const container = document.getElementById("avaliacoesRecebidasContainer");

  if (mediaAvaliacoes > 0) {
    resumo.innerHTML = `
      <div class="avaliacoes-summary">
        <div class="avaliacoes-rating">
          <span class="rating-number">${mediaAvaliacoes.toFixed(1)}</span>
          ${estrelasHTML(Math.round(mediaAvaliacoes), "rating-stars-large")}
          <span class="rating-count">${perfil.avaliacoes.length} avaliações</span>
        </div>
      </div>
    `;
  }

  if (!perfil.avaliacoes.length) {
    container.innerHTML = `
      <div class="empty-state">
        <i class="fa-regular fa-star"></i>
        <p>Nenhuma avaliação recebida ainda.</p>
        <small>As avaliações aparecerão aqui quando os clientes avaliarem seus serviços.</small>
      </div>
    `;
    return;
  }

  container.innerHTML = `
    <div class="avaliacoes-list">
      ${perfil.avaliacoes.map(av => `
        <div class="avaliacao-item">
          <div class="avaliacao-header">
            <div class="avaliacao-user">
              <div class="user-avatar">
                <i class="fa-solid fa-user"></i>
              </div>
              <div class="user-info">
                <span class="user-name">Cliente #${av.idFrete}</span>
                ${estrelasHTML(av.nota, "avaliacao-stars")}
              </div>
            </div>
            <span class="avaliacao-date">${av.dataAvaliacao || ""}</span>
          </div>

          ${av.comentario ? `
            <div class="avaliacao-comentario">
              <p>"${av.comentario}"</p>
            </div>
          ` : ""}

          ${av.foto ? `
            <div class="avaliacao-fotos">
              <div class="foto-thumbnail">
                <img src="${av.foto}" alt="Foto da entrega" class="foto-avaliacao">
              </div>
            </div>
          ` : ""}
        </div>
      `).join("")}
    </div>
  `;
}

function renderAvaliacoesFeitas() {
  if (tipoUsuario.toLowerCase() === "transportador") return;

  document.getElementById("avaliacoesFeitasCard").style.display = "block";
  const container = document.getElementById("avaliacoesFeitasContainer");

  if (!perfil.avaliacoesFeitas.length) {
    container.innerHTML = `
      <div class="empty-state">
        <i class="fa-regular fa-comment"></i>
        <p>Você ainda não fez nenhuma avaliação.</p>
        <small>As avaliações aparecerão aqui após você avaliar os fretes concluídos.</small>
      </div>
    `;
    return;
  }

  container.innerHTML = `
    <div class="avaliacoes-list">
      ${perfil.avaliacoesFeitas.map(av => `
        <div class="avaliacao-item">
          <div class="avaliacao-header">
            <div class="avaliacao-user">
              <div class="user-avatar">
                <i class="fa-solid fa-truck"></i>
              </div>
              <div class="user-info">
                <span class="user-name">Transportador</span>
                ${estrelasHTML(av.nota, "avaliacao-stars")}
              </div>
            </div>
            <span class="avaliacao-date">${av.dataAvaliacao || ""}</span>
          </div>

          ${av.comentario ? `
            <div class="avaliacao-comentario">
              <p>"${av.comentario}"</p>
            </div>
          ` : ""}

          ${av.foto ? `
            <div class="avaliacao-fotos">
              <div class="foto-thumbnail">
                <img src="${av.foto}" alt="Minha foto da entrega" class="foto-avaliacao">
              </div>
            </div>
          ` : ""}
        </div>
      `).join("")}
    </div>
  `;
}

function atualizarDddPorTelefone() {
  const telefone = document.getElementById("telefone").value;
  const match = telefone.match(/\(?(\d{2})\)?/);

  if (match) {
    document.getElementById("ddd").value = match[1];
  }
}

function salvarPerfil(event) {
  event.preventDefault();

  usuario.nome = document.getElementById("nome").value.trim();
  usuario.email = document.getElementById("email").value.trim();
  usuario.telefone = document.getElementById("telefone").value.trim();
  usuario.estado = document.getElementById("estado").value.trim();
  usuario.cidade = document.getElementById("cidade").value.trim();
  usuario.endereco = document.getElementById("endereco").value.trim();
  usuario.documento = document.getElementById("documento").value.trim();
  usuario.ddd = document.getElementById("ddd").value.trim();

  if (tipoUsuario.toLowerCase() === "transportador") {
    usuario.veiculo = document.getElementById("veiculo").value.trim();
  }

  preencherPerfil();
  mostrarAlertaSucesso("Perfil atualizado com sucesso!");
}

function abrirFoto(src) {
  document.getElementById("fotoModal").style.display = "block";
  document.getElementById("modalFoto").src = src;
}

function fecharFoto() {
  document.getElementById("fotoModal").style.display = "none";
}

function abrirModalExclusao() {
  const modal = document.getElementById("modalExcluirConta");
  modal.style.display = "flex";
  document.body.classList.add("modal-open");

  document.getElementById("confirmacaoTexto").value = "";
  document.getElementById("btnConfirmarExclusao").disabled = true;

  const lista = document.getElementById("listaExclusao");
  lista.innerHTML = `
    <li>Seu perfil e dados pessoais</li>
    <li>Todos os seus fretes</li>
    <li>Histórico de atividades</li>
    <li>Avaliações realizadas/recebidas</li>
    ${tipoUsuario.toLowerCase() === "transportador" ? "<li>Seus veículos cadastrados</li>" : ""}
  `;
}

function fecharModalExclusao() {
  document.getElementById("modalExcluirConta").style.display = "none";
  document.body.classList.remove("modal-open");
}

function confirmarExclusaoConta() {
  const confirmado = confirm("Tem certeza absoluta? Esta ação não pode ser desfeita.");
  if (!confirmado) return;

  alert("Conta excluída com sucesso! (simulação)");
  window.location.href = "index.html";
}

function bindEventos() {
  document.getElementById("perfilForm").addEventListener("submit", salvarPerfil);
  document.getElementById("telefone").addEventListener("input", atualizarDddPorTelefone);

  document.getElementById("btnExcluirConta").addEventListener("click", abrirModalExclusao);
  document.getElementById("fecharModalExclusaoBtn").addEventListener("click", fecharModalExclusao);
  document.getElementById("cancelarExclusaoBtn").addEventListener("click", fecharModalExclusao);
  document.getElementById("btnConfirmarExclusao").addEventListener("click", confirmarExclusaoConta);

  document.getElementById("confirmacaoTexto").addEventListener("input", function () {
    const valido = this.value.trim().toUpperCase() === "EXCLUIR CONTA";
    document.getElementById("btnConfirmarExclusao").disabled = !valido;
    this.style.borderColor = valido ? "#27ae60" : "#ddd";
  });

  document.getElementById("fecharFotoModal").addEventListener("click", fecharFoto);

  document.addEventListener("click", function (e) {
    if (e.target.classList.contains("foto-avaliacao")) {
      abrirFoto(e.target.src);
    }

    if (e.target.id === "fotoModal") {
      fecharFoto();
    }

    if (e.target.id === "modalExcluirConta") {
      fecharModalExclusao();
    }
  });

  document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") {
      fecharFoto();
      fecharModalExclusao();
    }
  });
}

preencherPerfil();
renderHistorico();
renderAvaliacoesRecebidas();
renderAvaliacoesFeitas();
bindEventos();