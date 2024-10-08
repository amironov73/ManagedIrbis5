// путь к бэк-энду
const baseURL = '/opac/'


let databases = [] // базы данных
let scenarios = [] // сценарии поиска
let inputRows = [] // строки со значениями поисковых атрибутов

let allOk = true // флаг: можно продолжать работать

const databaseSelector = document.getElementById('database-selector')
const inputContainer = document.getElementById('input-container')
const busyIndicator = document.getElementById('busy-indicator')
const errorIndicator = document.getElementById('error-indicator')
const mainForm = document.getElementById('main-form')
const resultContainer = document.getElementById('result-container')

function show(element, state) {
    element.style.display = state ? "initial" : "none"
}

function showBusy() {
    show(busyIndicator, true)
}

function hideBusy() {
    show(busyIndicator, false)
}

function showError(message) {
    errorMessage.innerText = message
    show(errorIndicator, true)
}

function hideError() {
    show(errorIndicator, false)
}

function buildTerm(row) {
    if (row.input.value) {
        return '"' + row.select.value + row.input.value + (row.check.checked ? '$' : '') + '"'
    }

    return ''
}

function buildExpression() {
    let expression = ''
    for (const row of inputRows) {
        let term = buildTerm(row)
        if (term) {
            expression = expression ? (expression + ' * ' + term) : term
        }
    }

    if (!expression) {
        return ''
    }

    const database = databaseSelector.value
    // noinspection UnnecessaryLocalVariableJS
    const result = baseURL + 'search/' + database + '/' + encodeURIComponent(expression)
    // console.log (result)

    return result
}

function handleSuccess(data) {
    // const documents = data.sort()
    const documents = data
    // console.log ('Найдено: ' + documents.length)

    if (documents.length === 0) {
        showError('Не найдено ни одного документа, удовлетворяющего заданным условиям')
        return
    }

    // let index = 0
    for (const book of documents) {
        const item = document.createElement('div')
        item.classList.add('found-card')
        item.classList.add('d-flex')
        item.classList.add('flex-row')

        if (book.arrangement) {
            const arrangement = document.createElement('div')
            arrangement.classList.add('arrangement')
            arrangement.innerHTML = book.arrangement
            item.appendChild(arrangement)
        }

        const coverUrl = book.cover
        if (coverUrl) {
            const cover = document.createElement('img')
            cover.src = coverUrl
            cover.width = 100
            item.appendChild(cover)
        }

        const description = document.createElement('div')
        description.style.setProperty('flex-grow', '1')
        description.style.setProperty('margin-left', '1em')
        description.innerHTML = book.description
        item.appendChild(description)

        if (book.links) {
            const linksDiv = document.createElement('div')
            linksDiv.classList.add('links')

            const label = document.createElement('span')
            label.classList.add('links-label')
            label.classList.add('text-primary')
            label.innerText = 'Ссылки:'
            linksDiv.appendChild(label)

            for (const link of book.links) {
                const one = document.createElement('a')
                one.classList.add('link')
                one.href = link.url
                one.target = '_blank'
                one.innerText = link.description ?? 'Ссылка'
                linksDiv.appendChild(one)
            }

            description.appendChild(linksDiv)
        }

        if (book.exemplars) {
            const exemplarsDiv = document.createElement('div')
            exemplarsDiv.classList.add('exemplar')

            const label = document.createElement('span')
            label.classList.add('exemplar-label')
            label.classList.add('text-primary')
            label.innerText = 'Экземпляры:'
            exemplarsDiv.appendChild(label)

            for (const exemplar of book.exemplars) {
                const area = document.createElement('span')
                switch (exemplar.status) {
                    case 'ok':
                        area.classList.add('exemplar-ok')
                        area.innerText = exemplar.number
                        break;

                    case 'u':
                        area.classList.add('exemplar-u')
                        area.innerText = exemplar.amount + ' экз.'
                        break;

                    default:
                        area.classList.add('exemplar-bad')
                        area.innerText = exemplar.number + ' (' + exemplar.onhand + ')'
                        break;
                }

                const sigla = document.createElement('span')
                sigla.classList.add('sigla')
                sigla.innerText = exemplar.sigla
                area.appendChild(sigla)

                exemplarsDiv.appendChild(area)
            }
            description.appendChild(exemplarsDiv)
        }

        resultContainer.appendChild(item)
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
    axios.get(url)
        .then(function (response) {
            handleSuccess(response.data)
            hideBusy()
        })
        .catch(function (error) {
            console.log(error)
            hideBusy()
            //showError ('Сервер не ответил либо прислал невалидный ответ')
            showError(error.message)
        })

    return false
}

function somethingWentWrong(error) {
    hideBusy()
    console.log(error)
    showError(error.message)
    allOk = false

    // запрещаем форму, точнее, все ее элементы
    for (const element of mainForm.elements) {
        element.disabled = true
        element.readOnly = true
    }
}

function createRow() {
    const inputGroup = document.createElement('div')
    inputGroup.classList.add('input-group')

    const select = document.createElement('select')
    select.classList.add('btn')
    select.classList.add('btn-outline-primary')
    select.style.setProperty('text-align', 'left')
    inputGroup.appendChild(select)

    const input = document.createElement('input')
    input.classList.add('form-control')
    input.type = 'text'
    inputGroup.appendChild(input)

    const div = document.createElement('div')
    div.classList.add('input-group-text')
    inputGroup.appendChild(div)

    const check = document.createElement('input')
    check.classList.add('form-check-input')
    check.type = 'checkbox'
    check.checked = true
    check.title = 'усечение'
    // check.setAttribute ('data-bs-toggle', 'tooltip')
    check.setAttribute('data-bs-placement', 'top')
    // new Tooltip (check)
    div.appendChild(check)

    inputContainer.appendChild(inputGroup)

    const row = {
        group: inputGroup,
        select: select,
        input: input,
        check: check
    }
    inputRows.push(row)
}

function fillTheRow(row, indexToSelect) {
    const select = row.select
    select.innerHTML = ''
    let index = 0
    for (const scenario of scenarios) {
        const option = document.createElement('option')
        if (index === indexToSelect) {
            option.selected = true
        }
        index++
        option.value = scenario.prefix
        option.innerText = scenario.description
        select.appendChild(option)
    }
}

function fillSearchAttributes() {
    let indexToSelect = 0
    for (const row of inputRows) {
        fillTheRow(row, indexToSelect)
        indexToSelect++
        if (indexToSelect >= scenarios.length) {
            indexToSelect = 0
        }
    }
}

function addRow() {
    createRow()
    fillTheRow(inputRows [inputRows.length - 1], 0)

    return false
}

setTimeout(function () {

    // получаем список баз данных
    showBusy()
    axios.get(baseURL + 'databases')
        .then(function (response) {
            let first = true
            databases = response.data
            for (const db of databases) {
                const option = document.createElement('option')
                option.value = db.name
                option.innerText = db.name + ' - ' + db.description
                if (first) {
                    option.selected = true
                }
                first = false
                databaseSelector.appendChild(option)
            }
        })
        .catch(function (error) {
            somethingWentWrong(error)
        })
        .then(function () {
            if (allOk) {
                axios.get(baseURL + 'scenarios/' + databases[0].name)
                    .then(function (response) {
                        scenarios = response.data
                        fillSearchAttributes()
                        hideBusy()
                    })
                    .catch(function (error) {
                        somethingWentWrong(error)
                    })
            }
        })

    if (allOk) {
        // по умолчанию у нас три строки
        createRow()
        createRow()
        createRow()

        // возможность добавления строк с поисковыми атрибутами
        document.getElementById('add-row').addEventListener('click', addRow)

        mainForm.onsubmit = handleSubmit
    }

})
