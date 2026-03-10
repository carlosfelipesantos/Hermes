document.getElementById("cadastrarVeiculoForm").addEventListener("submit", function (e) {
  e.preventDefault();

  const veiculos = JSON.parse(localStorage.getItem("veiculos")) || [];

  const novoVeiculo = {
    id: Date.now(),
    tipoVeiculo: document.getElementById("tipoVeiculo").value,
    marca: document.getElementById("marca").value.trim(),
    modelo: document.getElementById("modelo").value.trim(),
    ano: Number(document.getElementById("ano").value),
    placa: document.getElementById("placa").value.trim().toUpperCase(),
    capacidade: Number(document.getElementById("capacidade").value),
    cor: document.getElementById("cor").value.trim(),
    dataCadastro: new Date().toISOString().split("T")[0]
  };

  veiculos.push(novoVeiculo);
  localStorage.setItem("veiculos", JSON.stringify(veiculos));

  alert("Veículo cadastrado com sucesso!");
  window.location.href = "meus-veiculos.html";
});