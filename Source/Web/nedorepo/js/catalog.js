//const baseURL = 'http://elib.istu.edu/Jsoner.php'
const baseURL = 'http://localhost/Jsoner.php'

let scenarios = [] // сценарии поиска
let inputRows = [] // строки со значениями поисковых атрибутов

const databaseSelector = document.getElementById ('database-selector')
const inputContainer   = document.getElementById ('input-container')
const formatSelector   = document.getElementById ('format-selector')
const howMany          = document.getElementById ('how-many')
const fullText         = document.getElementById ('full-text')
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

function addRow() {
    createRow()
    fillTheRow (inputRows [inputRows.length - 1], 0)
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
    result = baseURL + '?op=search_format&db=' +  database + '&expr=' + encodeURIComponent (expression) + '&format=' + format + "&limit=" + limit
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
        item.innerHTML = description
        resultContainer.appendChild (item)
    }
}

function handleSubmit() {
    hideError()
    resultContainer.innerHTML = ''
    const url = buildExpression()
    showBusy()
    axios.get (url)
        .then (function (response) {
            handleSuccess (response.data)
            hideBusy ()
        })
        .catch (function (error) {
            console.log(error)
            hideBusy ()
            showError ('Сервер не ответил либо прислал невалидный ответ')
        })

    return false
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

setTimeout (function () {

    // получаем список баз данных
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
        .then (function () {
            axios.get (baseURL + '?op=scenarios')
                .then(function (response) {
                    scenarios = response.data
                    fillSearchAttributes()
                })
        })

    createRow()
    createRow()
    createRow()
})
