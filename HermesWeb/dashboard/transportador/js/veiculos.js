let veiculos = JSON.parse(localStorage.getItem("veiculos")) || [
  {
    id: 1,
    tipoVeiculo: "Caminhão",
    marca: "Mercedes",
    modelo: "Atego",
    ano: 2020,
    placa: "ABC-1234",
    capacidade: 5000,
    cor: "Branco",
    dataCadastro: "2026-02-20"
  }
];

const vehiclesContainer = document.getElementById("vehiclesContainer");

function renderVeiculos() {
  vehiclesContainer.innerHTML = "";

  if (!veiculos.length) {
    vehiclesContainer.innerHTML = `
      <div class="vehicles-empty">
        <i class="fas fa-truck fa-4x"></i>
        <h3>Nenhum veículo cadastrado</h3>
        <p>Cadastre seu primeiro veículo para começar a aceitar fretes.</p>
      </div>
    `;
    return;
  }

  const grid = document.createElement("div");
  grid.className = "vehicles-grid";

  veiculos.forEach(v => {
    const card = document.createElement("div");
    card.className = "vehicle-card";

    card.innerHTML = `
      <div class="vehicle-card-header">
        <h3>
          <i class="fas fa-truck"></i>
          ${v.marca} ${v.modelo}
        </h3>

        <span class="vehicle-plate">${v.placa}</span>
      </div>

      <div class="vehicle-info">
        <p><strong>Tipo:</strong> ${v.tipoVeiculo}</p>
        <p><strong>Ano:</strong> ${v.ano}</p>
        <p><strong>Capacidade:</strong> ${Number(v.capacidade).toFixed(2)} kg</p>
        <p><strong>Cor:</strong> ${v.cor || "Não informada"}</p>
        <p><strong>Cadastrado em:</strong> ${v.dataCadastro || ""}</p>
      </div>

      <div class="vehicle-actions">
        <a href="editar-veiculo.html?id=${v.id}" class="btn-edit">
          <i class="fas fa-edit"></i> Editar
        </a>

        <button class="btn-delete" data-id="${v.id}">
          <i class="fas fa-trash"></i> Excluir
        </button>
      </div>
    `;

    grid.appendChild(card);
  });

  vehiclesContainer.appendChild(grid);
  bindDeleteEvents();
}

function bindDeleteEvents() {
  document.querySelectorAll(".btn-delete").forEach(btn => {
    btn.addEventListener("click", function () {
      const id = Number(this.dataset.id);
      const confirmar = confirm("Tem certeza que deseja excluir este veículo?");

      if (!confirmar) return;

      veiculos = veiculos.filter(v => v.id !== id);
      localStorage.setItem("veiculos", JSON.stringify(veiculos));
      renderVeiculos();
    });
  });
}

renderVeiculos();