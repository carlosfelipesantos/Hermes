// Simulação de dados do usuário logado
// Você pode trocar isso depois por dados vindos de API, localStorage ou login real
const usuario = {
  nome: "Thales",
  tipo: "cliente" // pode ser: "cliente", "transportador" ou null
};

const logado = usuario.nome !== null && usuario.tipo !== null;

const heroButtons = document.getElementById("heroButtons");
const navActions = document.getElementById("navActions");

function renderButtons() {
  if (!heroButtons) return;

  if (logado) {
    if (usuario.tipo.toLowerCase() === "cliente") {
      heroButtons.innerHTML = `
        <a href="fretes/listaFretes.html" class="btn btn-large btn-primary">
          <i class="fas fa-box"></i>
          Lista de Fretes
        </a>
        <a href="dashboard/clientes/clientes.html" class="btn btn-large btn-secondary">
          <i class="fas fa-user"></i>
          Ver Painel
        </a>
      `;
    } else if (usuario.tipo.toLowerCase() === "transportador") {
      heroButtons.innerHTML = `
        <a href="fretes/listaFretesTransportador.html" class="btn btn-large btn-primary">
          <i class="fas fa-truck-loading"></i>
          Lista de Fretes
        </a>
        <a href="dashboardTransportador.html" class="btn btn-large btn-secondary">
          <i class="fas fa-tachometer-alt"></i>
          Ver Painel
        </a>
      `;
    }
  } else {
    heroButtons.innerHTML = `
      <a href="auth/login/login.html" class="btn btn-large btn-primary">
        <i class="fas fa-box"></i>
        Preciso de um Frete
      </a>
      <a href="auth/login/login.html" class="btn btn-large btn-secondary">
        <i class="fas fa-truck"></i>
        Sou Transportador
      </a>
    `;
  }
}

function renderNavbar() {
  if (!navActions) return;

  if (logado) {
    navActions.innerHTML = `
      <span class="user-name">Olá, ${usuario.nome}</span>
      <a href="#" class="btn btn-secondary" id="logoutBtn">Sair</a>
    `;

    const logoutBtn = document.getElementById("logoutBtn");
    logoutBtn?.addEventListener("click", (e) => {
      e.preventDefault();
      alert("Logout simulado.");
    });
  } else {
    navActions.innerHTML = `
      <a href="auth/login/login.html" class="btn btn-secondary">Entrar</a>
    `;
  }
}

renderButtons();
renderNavbar();