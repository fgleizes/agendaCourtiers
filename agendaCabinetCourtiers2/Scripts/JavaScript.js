
function displayInputValidationError(input) {
    let fieldValidation = input.nextElementSibling
    if (fieldValidation.firstChild) {
        fieldValidation.removeChild(fieldValidation.firstChild)
    }

    let error
    if (input.dataset.valRequired && input.value == "") {
        let span = document.createElement("span");
        error = input.dataset.valRequired
        span.innerHTML = error
        fieldValidation.append(span)

    } else if (input.dataset.valRegexPattern && !input.value.match(input.dataset.valRegexPattern)) {
        let span = document.createElement("span");
        error = input.dataset.valRegex
        span.innerHTML = error
        fieldValidation.append(span)

    } else if (input.dataset.valLengthMax && input.value.length > 50) {
        let span = document.createElement("span");
        error = input.dataset.valLength
        span.innerHTML = error
        fieldValidation.append(span)
    }
    return error
}

function displayFormValidationErrors(errors) {
    let ul = document.querySelector('div.validation-summary-valid.text-danger > ul')
    while (ul.firstChild) {
        ul.removeChild(ul.firstChild)
    }
       
    // AFFICHAGE DE LA LISTE D'ERREURS EN HAUT DU FORMULAIRE
    /*errors.forEach(function (errorMessage) {
        let li = document.createElement('li')
        li.innerHTML = errorMessage
        ul.appendChild(li)
    })*/

    // OU AFFICHAGE D'UN MESSAGE EN HAUT DU FORMULAIRE
    let li = document.createElement('li')
    li.innerHTML = "Veuillez corriger les erreurs et re-valider le formulaire."
    ul.appendChild(li)
}


let showModalButtons = document.querySelectorAll('.showModal')
if (showModalButtons.length) {
    showModalButtons.forEach(showModalButton => showModalButton.addEventListener('click', e => {
        e.preventDefault()
        let url = e.currentTarget.href

        let xhr = null

        if (window.XMLHttpRequest || window.ActiveXObject) {
            if (window.ActiveXObject) {
                try {
                    xhr = new ActiveXObject("Msxml2.XMLHTTP")
                } catch (e) {
                    xhr = new ActiveXObject("Microsoft.XMLHTTP")
                }
            } else {
                xhr = new XMLHttpRequest()
            }
        } else {
            alert("Votre navigateur ne supporte pas l'objet XMLHTTPRequest...")
            return
        }

        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                new bootstrap.Modal(document.querySelector('.modal')).show()
                document.getElementById('contentModal').innerHTML = xhr.responseText



                let inputs = document.querySelectorAll('.editForm .form-control')

                inputs.forEach(input => input.addEventListener('blur', (e) => {
                    if (displayInputValidationError(e.currentTarget)) {
                        e.currentTarget.addEventListener('input', (e) => {
                            displayInputValidationError(e.currentTarget)
                        })
                    }
                }))

                document.querySelector('.editForm').addEventListener('submit', (e) => {
                    e.preventDefault()
                    let url = e.currentTarget.action
                    let formValues = new FormData(e.currentTarget)

                    let xhr = null

                    if (window.XMLHttpRequest || window.ActiveXObject) {
                        if (window.ActiveXObject) {
                            try {
                                xhr = new ActiveXObject("Msxml2.XMLHTTP")
                            } catch (e) {
                                xhr = new ActiveXObject("Microsoft.XMLHTTP")
                            }
                        } else {
                            xhr = new XMLHttpRequest()
                        }
                    }
                    xhr.overrideMimeType("application/json");
                    xhr.onreadystatechange = function () {
                        if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
                            let data = JSON.parse(xhr.responseText)
                            if (data.success == true) {
                                window.location.reload(true)
                            } else {
                                displayFormValidationErrors(data.errors)

                                inputs.forEach(input => {
                                    displayInputValidationError(input)
                                    input.addEventListener('input', (e) => {
                                        displayInputValidationError(e.currentTarget)
                                        let errors = []
                                        document.querySelectorAll('.field-validation-valid span').forEach(span => errors.push(span.textContent))
                                        displayFormValidationErrors(errors)
                                    })
                                })
                            }
                        }
                    }

                    xhr.open("post", url, true);
                    xhr.send(formValues);
                })
            }
        }

        xhr.open("GET", url, true);
        xhr.send();
    }))

    document.querySelector('.modal').addEventListener('hidden.bs.modal', () => {
        let contentModal = document.getElementById('contentModal')
        while (contentModal.firstChild) {
            contentModal.removeChild(contentModal.firstChild)
        }
    })
}