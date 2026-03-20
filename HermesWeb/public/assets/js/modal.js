
function abrirModal(idFrete) {
    fetch(`FreteDetalhesServlet?idFrete=${idFrete}`)
        .then(resp => resp.text())
        .then(html => {
            document.body.insertAdjacentHTML("beforeend", html);
        });
}

function fecharModal() {
    const modal = document.querySelector(".modal-overlay");
    if (modal) modal.remove();
}
