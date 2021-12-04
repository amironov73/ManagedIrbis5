<script>
    import InputLine from "./InputLine.svelte"
    import {prefixes} from "./prefix";

    let first = '', second = '', third = ''
    let searchExpression = ''
    let allFound = ''
    let foundMessage = 'Попробуйте фамилии: Байбородин, Бегунов, Большаков, Гридин, Зайдес, Лобацкая, Меерович, Пономарев, Тен, Щадов, Ястребов'

    function formatLine(line) {
        if (line.value) {

            return '"' + line.prefix + line.value + (line.checked ? '$' : '') + '"'
        }

        return ''
    }

    function formatAll() {
        let result = ''
        let one = formatLine(first)
        if (one) {
            result = one
        }

        one = formatLine(second)
        if (one) {
            result = result ? result + ' * ' + one : one
        }

        one = formatLine(third)
        if (one) {
            result = result ? result + ' * ' + one : one
        }

        // if (result === '') {
        //     return result
        // }
        //
        // //result = '"VRL=ASP" * "TEK=http://elib.$" * (' + result + ')'
        // result = '"VRL=ASP" * "TEK=http://elib.$"'

        return result
    }

    function getPrefix(index) {
        if (index >= prefixes.length) {
            index = 0
        }

        return prefixes[index].prefix
    }

    function searchArticles(expression) {
        allFound = ''
        foundMessage = ''

        if (expression === '') {
            return
        }

        const url = 'https://localhost:5001/search_format/VRL=ASP+*+TEK=HTTP:%2F%2F$+*+' + expression + '/@brief/PERIO/100'
        console.log(url)

        my.getJSON(url, function (books) {
            allFound = books
            if (allFound.length === 0) {
                foundMessage = 'Ничего не найдено'
            }
        })
    }

</script>

<div class="container">
    <div style="margin-bottom: 5pt;">
        <div class="d-flex justify-content-center">
            <h1>Репозиторий статей сотрудников ИРНИТУ</h1>
        </div>
    </div>

    <div style="margin: 5pt auto;">
        <div style="max-width: 600px;">
            <InputLine bind:this={first} prefix={getPrefix(0)}/>
        </div>
        <div style="max-width: 600px;">
            <InputLine bind:this={second} prefix={getPrefix(1)}/>
        </div>
        <div style="max-width: 600px;">
            <InputLine bind:this={third} prefix={getPrefix(2)}/>
        </div>
    </div>

    <div style="margin: 10pt 0pt;">
        <div class="d-flex justify-content-center">
            <button class="btn btn-primary"
                    on:click={() => searchArticles (formatAll())}>
                Поиск
            </button>
        </div>
    </div>
    <div>
        <div>
            {foundMessage}
        </div>
        <table>
            {#each allFound as item, index}
                <tr>
                    <td style="min-width: 30px;">{index + 1}</td>
                    <td>
                        {item}
                    </td>
                    <td>
                        <a href="#">
                            <img src="pdf.gif" alt="Ссылка">
                        </a>
                    </td>
                </tr>
            {/each}
        </table>
    </div>

</div>

<style>
td { vertical-align: top; padding: 3px; border: #0b5ed7 thin solid; }
</style>
