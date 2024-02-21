// отображение служебной информации
const busyIndicator  = document.getElementById ('busyIndicator')
const errorIndicator = document.getElementById ('errorIndicator')
const errorMessage   = document.getElementById ('errorMessage')
const resultHolder   = document.getElementById ('resultHolder')

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

function showResult(text) {
    resultHolder.innerText = text
}

function buildUrl (expression) {
    const resource = 'http://elib.istu.edu/Jsoner.php?op=search_format'
    const database = 'ISTU'
    const format = '@brief'
    // noinspection UnnecessaryLocalVariableJS
    const result = resource + '&db=' +  database + '&format=' + format + '&expr=' + encodeURIComponent (expression)
    // console.log (result)

    return result
}

function handleSuccess (data) {
    const books = data.sort()
    // console.log ('Найдено: ' + books.length)

    if (books.length === 0) {
        showError ('Не найдено ни одной книги, удовлетворяющей заданным условиям')
        return
    }

    let index = 0
    for (const book of books) {
        const paragraph = document.createElement ('p')
        paragraph.innerText = book
        resultHolder.appendChild (paragraph)
    }
}

function performSearch(expression) {
    const url = buildUrl (expression)
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
}

function handleSubmit() {
    showResult('')
    hideError()

    const expression = document.getElementById('input1').value
    if (!expression) {
        return false
    }

    performSearch(expression)
    return false
}
