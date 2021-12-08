import '@popperjs/core/dist/umd/popper'
import bootstrap from 'bootstrap/dist/js/bootstrap.bundle'
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css'
import './../css/styles.css'
import './../img/arctic-fox.gif'
import './../img/favicon.ico'
import './../img/logo.png'

// путь к бэк-энду
const baseURL = 'http://elib.istu.edu/Jsoner.php'

let scenarios = [] // сценарии поиска
let inputRows = [] // строки со значениями поисковых атрибутов

let allOk = true // флаг: можно продолжать работать

const databaseSelector = document.getElementById ('database-selector')
const inputContainer   = document.getElementById ('input-container')
const howMany          = document.getElementById ('how-many')
const formatSelector   = document.getElementById ('format-selector')
const fullText         = document.getElementById ('full-text')
const busyIndicator    = document.getElementById ('busy-indicator')
const errorIndicator   = document.getElementById ('error-indicator')
const resultContainer  = document.getElementById ('result-container')

function show (element, state) {
    element.style.display = state ? "initial" : "none"
}

function showBusy() {
    show (busyIndicator, true)
}

function hideBusy() {
    show (busyIndicator, false)
}

function showError (message) {
    errorMessage.innerText = message
    show (errorIndicator, true)
}

function hideError() {
    show (errorIndicator, false)
}

function buildTerm (row) {
    if (row.input.value) {
        return '"' + row.select.value + row.input.value + (row.check.checked ? '$' : '') + '"'
    }

    return ''
}

function buildExpression() {
    let expression = ''
    for (const row of inputRows) {
        let term = buildTerm (row)
        if (term) {
            expression = expression ? (expression + ' * ' + term) : term
        }
    }

    if (!expression) {
        return ''
    }

    const database = databaseSelector.value
    const format = formatSelector.value
    const limit = howMany.value
    const result = baseURL + '?op=search_format&db=' +  database + '&expr=' + encodeURIComponent (expression) + '&format=' + format + "&limit=" + limit
    // console.log (result)

    return result
}

function handleSuccess (data) {
    const documents = data.sort()
    // console.log ('Найдено: ' + documents.length)

    if (documents.length === 0) {
        showError ('Не найдено ни одного документа, удовлетворяющего заданным условиям')
        return
    }

    // let index = 0
    for (const description of documents) {
        const item = document.createElement ('div')
        item.classList.add ('card')
        item.innerHTML = description
        resultContainer.appendChild (item)
    }
}

function handleSubmit() {
    hideError()
    resultContainer.innerHTML = ''
    const url = buildExpression()
    if (!url) {
        return false
    }

    showBusy()
    axios.get (url)
        .then (function (response) {
            handleSuccess (response.data)
            hideBusy()
        })
        .catch (function (error) {
            console.log (error)
            hideBusy()
            //showError ('Сервер не ответил либо прислал невалидный ответ')
            showError (error.message)
        })

    return false
}

function somethingWentWrong (error) {
    console.log (error)
    showError (error.message)
    allOk = false

    // запрещаем форму, точнее, все ее элементы
    const form = document.getElementById ('main-form')
    for (const element of form.elements) {
        element.disabled = true
        element.readOnly = true
    }
}

function createRow() {
    const inputGroup = document.createElement ('div')
    inputGroup.classList.add ('input-group')

    const select = document.createElement ('select')
    select.classList.add ('btn')
    select.classList.add ('btn-outline-primary')
    inputGroup.appendChild (select)

    const input = document.createElement ('input')
    input.classList.add ('form-control')
    input.type = 'text'
    inputGroup.appendChild (input)

    const div = document.createElement ('div')
    div.classList.add ('input-group-text')
    inputGroup.appendChild (div)

    const check = document.createElement ('input')
    check.classList.add ('form-check-input')
    check.type = 'checkbox'
    check.checked = true
    check.title = 'усечение'
    check.setAttribute ('data-bs-toggle', 'tooltip')
    check.setAttribute ('data-bs-placement', 'top')
    new bootstrap.Tooltip (check)
    div.appendChild (check)

    inputContainer.appendChild (inputGroup)

    const row = {
        group: inputGroup,
        select: select,
        input: input,
        check: check
    }
    inputRows.push (row)
}

function fillTheRow (row, indexToSelect) {
    const select = row.select
    select.innerHTML = ''
    let index = 0
    for (const scenario of scenarios) {
        const option = document.createElement ('option')
        if (index === indexToSelect) {
            option.selected = true
        }
        index++
        option.value = scenario.prefix
        option.innerText = scenario.name
        select.appendChild (option)
    }
}

function fillSearchAttributes() {
    let indexToSelect = 0
    for (const row of inputRows) {
        fillTheRow (row, indexToSelect)
        indexToSelect++
        if (indexToSelect >= scenarios.length) {
            indexToSelect = 0
        }
    }
}

function addRow() {
    createRow()
    fillTheRow (inputRows [inputRows.length - 1], 0)

    return false
}

setTimeout(function () {

    const addRowButton = document.getElementById ('add-row')
    addRowButton.onclick = addRow
    new bootstrap.Tooltip (addRowButton)

    const mainForm = document.getElementById ('main-form')
    mainForm.onsubmit = handleSubmit

    // получаем список баз данных
    showBusy()
    axios.get (baseURL + '?op=list_db&spec=1..dbnam3.mnu')
        .then (function (response) {
            let first = true
            for (const db of response.data) {
                const option = document.createElement ('option')
                option.value = db.name
                option.innerText = db.name + ' - ' + db.description
                if (first) {
                    option.selected = true
                }
                first = false
                databaseSelector.appendChild (option)
            }
        })
        .catch (function (error) {
            somethingWentWrong (error)
        })
        .then (function () {
            if (allOk) {
                axios.get(baseURL + '?op=scenarios')
                    .then(function (response) {
                        scenarios = response.data
                        fillSearchAttributes()
                        hideBusy()
                    })
                    .catch(function (error) {
                        somethingWentWrong (error)
                    })
            }
        })

    if (allOk) {
        // по умолчанию у нас три строки
        createRow()
        createRow()
        createRow()
    }
})
