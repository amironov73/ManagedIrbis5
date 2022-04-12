// элементы пользовательского ввода
const select1  = document.getElementById ('select1')
const input1   = document.getElementById ('input1')
const check1   = document.getElementById ('check1')
const select2  = document.getElementById ('select2')
const input2   = document.getElementById ('input2')
const check2   = document.getElementById ('check2')
const select3  = document.getElementById ('select3')
const input3   = document.getElementById ('input3')
const check3   = document.getElementById ('check3')
const howMany  = document.getElementById ('howMany')
const fullText = document.getElementById ('fullText')

// отображение служебной информации
const busyIndicator  = document.getElementById ('busyIndicator')
const suggestion     = document.getElementById ('suggestion')
const errorIndicator = document.getElementById ('errorIndicator')
const errorMessage   = document.getElementById ('errorMessage')
const debugIndicator = document.getElementById ('debugIndicator')
const debugMessage   = document.getElementById ('debugMessage')

// список найденных статей
const articleList = document.getElementById ('articleList')

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

// noinspection JSUnusedGlobalSymbols
function showDebug (message) {
    debugMessage.innerText = message
    show (debugIndicator, true)
}

function hideSuggestion() {
    show (suggestion, false)
}

function hideDebug() {
    show (debugIndicator, false)
}

function clearArticleList() {
    articleList.innerHTML = ''
}

function splitToUrl (sourceText) {
    const http = '. - http'
    const index = sourceText.indexOf (http)
    if (index < 0) {
        return [sourceText, '']
    }

    const firstPart = sourceText.substr (0, index).trim()
    const secondPart = sourceText.substr (index + 4).trim()
    return [firstPart, secondPart]
}

function buildTerm (select, input, check) {
    if (input.value) {
        return '"' + select.value + input.value + (check.checked ? '$' : '') + '"'
    }

    return ''
}

function buildExpression() {
    let result = ''
    let term = buildTerm (select1, input1, check1)
    if (term) {
        result = term
    }

    term = buildTerm (select2, input2, check2)
    if (term) {
        result = result ? (result + ' * ' + term) : term
    }

    term = buildTerm (select3, input3, check3)
    if (term) {
        result = result ? (result + ' * ' + term) : term
    }

    return result
}

function buildUrl (expression) {
    // const resource = 'http://localhost:5000/search_format/'
    const resource = 'http://elib.istu.edu/Jsoner.php?op=search_format'
    const database = 'PERIO'
    const format = '@brief_with_url'
    const limit = howMany.value
    const filter = fullText.checked ? 'VRL=ASP+*+TEK=HTTP:%2F%2F$+*+' : 'VRL=ASP+*+'
    // const result = resource + filter + encodeURIComponent (expression) + '/' + format + '/' + database + '/' + limit
    // noinspection UnnecessaryLocalVariableJS
    const result = resource + '&db=' +  database + '&expr=' + filter + encodeURIComponent (expression) + '&format=' + format + "&limit=" + limit
    // console.log (result)

    return result
}

function buildUrl2 (expression) {
    const resource = 'http://elib.istu.edu/Jsoner.php?op=search_format'
    const database = 'ISTU'
    const format = '@brief_with_url'
    const limit = howMany.value
    const filter = fullText.checked ? 'TEK=HTTP:%2F%2F$+*+' : ''
    const result = resource + '&db=' +  database + '&expr=' + filter + encodeURIComponent (expression) + '&format=' + format + "&limit=" + limit

    return result
}

function handleSuccess (data) {
    const articles = data.sort()
    // console.log ('Найдено: ' + articles.length)

    if (articles.length === 0) {
        showError ('Не найдено ни одной статьи, удовлетворяющей заданным условиям')
        return
    }

    let index = 0
    for (const article of articles) {
        const parts = splitToUrl (article)

        const tr = document.createElement ('tr')

        const td1 = document.createElement ('td')
        td1.classList.add ('article-number')
        td1.innerText = (++index).toString()

        const td2 = document.createElement ('td')
        td2.innerText = parts[0]

        const td3 = document.createElement ('td')
        td3.classList.add ('article-icon')
        const link = parts[1]
        td3.innerHTML = link
            ? '<a href="' + link + '" target="_blank" title="полный текст статьи">' +
            '<img src="img/pdf.gif" alt="полный текст статьи"></a>'
            : '&nbsp;'

        tr.appendChild (td1)
        tr.appendChild (td2)
        tr.appendChild (td3)
        articleList.appendChild (tr)
    }
}

function searchForArticles (expression) {
    const url = buildUrl (expression)

    showBusy()

    // $.getJSON (url)
    //     .done (function (response) {
    //         handleSuccess (response)
    //     })
    //     .fail (function (error) {
    //         console.log (error)
    //         showError ('Сервер не ответил либо прислал невалидный ответ')
    //     })

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

function searchForArticles2 (expression) {
    const url = buildUrl2 (expression)
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
    showBusy()
    clearArticleList()
    hideSuggestion()
    hideDebug()
    hideError()

    const expression = buildExpression()
    if (!expression) {
        return
    }

    // showDebug (expression)

    searchForArticles (expression)
    searchForArticles2 (expression)

    return false
}

const tooltipTriggerList = [].slice.call (document.querySelectorAll ('[data-bs-toggle="tooltip"]'))
const tooltipList = tooltipTriggerList.map (function (tooltipTriggerEl) {
    return new bootstrap.Tooltip (tooltipTriggerEl)
})
