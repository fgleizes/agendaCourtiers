$(function () {
    function displayInputValidationErrors(inputs, errors = []) {

        inputs.forEach(input => {
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

            if (error) {
                errors.push(error)
            }
        })

        return errors
    }

    function displayFormValidationErrors(errors) {
        if (errors) {
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
            li.innerHTML = "Veuillez corriger les erreurs et valider le formulaire."
            ul.appendChild(li)
        }
    }

    document.querySelectorAll('.showModal').forEach(showModalButton => showModalButton.addEventListener('click', e => {
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

                document.getElementById('contentModal').innerHTML = xhr.responseText

                let inputs = document.querySelectorAll('.editForm .form-control')
                inputs.forEach(input => input.addEventListener('input', () => {
                    let errors = displayInputValidationErrors(inputs)

                    // AFFICHAGE DES ERREURS EN HAUT DU FORMULAIRE
                    /*displayFormValidationErrors(errors)*/
                }))

                $(".editForm").on("submit", function (e) {
                    e.preventDefault()
                    let url = $(e.currentTarget).attr('action')
                    let formValues = $(e.currentTarget).serialize();

                    $.ajax({
                        url: url,
                        type: 'POST',
                        dataType: 'json',
                        data: formValues,
                        success: function (data) {
                            if (data.success == true) {
                                window.location.reload(true)
                            } else {
                                displayInputValidationErrors(inputs)
                                displayFormValidationErrors(data.errors)
                            }
                        },
                        error: function () {
                            console.log("xhr error")
                        }
                    })
                })
            }
        };

        xhr.open("GET", url, true);
        xhr.send();

        /*$.get(url, function (data) {
            document.getElementById('contentModal').innerHTML = data

            let inputs = document.querySelectorAll('.editForm .form-control')
            inputs.forEach(input => input.addEventListener('input', () => {
                let errors = displayInputValidationErrors(inputs)

                // AFFICHAGE DES ERREURS EN HAUT DU FORMULAIRE
                *//*displayFormValidationErrors(errors)*//*
            }))

            $(".editForm").on("submit", function (e) {
                e.preventDefault()
                let url = $(e.currentTarget).attr('action')
                let formValues = $(e.currentTarget).serialize();

                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    data: formValues,
                    success: function (data) {
                        if (data.success == true) {
                            window.location.reload(true)
                        } else {
                            displayInputValidationErrors(inputs)
                            displayFormValidationErrors(data.errors)
                        }
                    },
                    error: function () {
                        console.log("xhr error")
                    }
                })
            })
        })*/
    }))

    /*$(".modal").on("hidden.bs.modal", () => $("#contentModal").empty())*/
    document.querySelector('.modal').addEventListener('hidden.bs.modal', () => {
        let contentModal = document.getElementById('contentModal')
        while (contentModal.firstChild) {
            contentModal.removeChild(contentModal.firstChild)
        }
    })
})