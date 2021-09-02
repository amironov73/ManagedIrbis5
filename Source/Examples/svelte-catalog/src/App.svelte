<script>
    const baseUri = 'https://localhost:5001/api?'
    let databases = []
    let currentDatabase
    let scenarios = [
        {"name": "Ключевое слово", "prefix": "K="},
        {"name": "Автор", "prefix": "A="},
        {"name": "Заглавие", "prefix": "T="}
    ]
    let amount = [10, 50, 100, 500, 1000, 5000]
    let howMany = amount[1]
    let currentPrefix = scenarios[0].prefix
    let valueToSearch
    let truncate = true
    let foundLines = []
    let busy = false
    let errorMessage;

    fetch(baseUri + 'op=list_db')
        .then(response => response.json())
        .then(data => {
            databases = data
            currentDatabase = databases[0].name
        })
        .catch(error => errorMessage = error.message)

    function doSearch() {
        if (valueToSearch && currentDatabase && currentPrefix) {
            const dollar = truncate ? '$' : '';
            const url = baseUri + `op=search_format`
                + `&db=${currentDatabase}`
                + `&format=@brief`
                + `&count=${howMany}`
                + `&expr="${currentPrefix}${valueToSearch}${dollar}"`

            errorMessage = false
            busy = true

            fetch(url)
                .then(response => response.json())
                .then(data => {
                    foundLines = data.sort()
                    busy = false
                    errorMessage = null
                })
                .catch(error => {
                    busy = false
                    errorMessage = error.message
                })
        }
    }

</script>

<style>
    .error {
        min-height: 3em;
        display: flex;
        align-items: center;
        font-weight: bold;
        justify-content: center;
        width: 100%;
        background-color: red;
        color: white;
    }

    li {
        padding: 1em;
    }
</style>

База данных:
<select bind:value={currentDatabase}>
    {#each databases as db}
        <option value={db.name}>{db.description}</option>
    {/each}
</select>

<br/>
Поиск по:
<select bind:value={currentPrefix}>
    {#each scenarios as scenario}
        <option value={scenario.prefix}>{scenario.name}</option>
    {/each}
</select>

<br/>
Показать:
<select bind:value={howMany}>
    {#each amount as one}
        <option value={one}>{one}</option>
    {/each}
</select>

<br/>
Искомое значение
<form>
    <input type="text" bind:value={valueToSearch}>
    <input id="truncateCheckbox" type="checkbox"
           bind:checked={truncate}>
    <label for="truncateCheckbox">Усечение</label>
    <button type="submit" on:click|preventDefault={doSearch}>Поиск</button>
</form>

<br/>
{#if errorMessage}
    <div class="error">{errorMessage}</div>
{/if}
{#if busy}
    <img src="spinner.gif" alt="Подождите">
{/if}
<ol>
    {#each foundLines as line}
        <li>{line}</li>
    {:else}
        Список пуст
    {/each}
</ol>