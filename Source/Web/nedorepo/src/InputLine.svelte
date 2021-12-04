<script>
    import {prefixes} from "./prefix"

    export let prefix = ''
    export let value = ''
    export let checked = true

    function uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            const r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString (16)
        });
    }

    const uniqueId = uuidv4()

</script>

<svelte:options accessors={true}/>

<div class="input-group">
    <select bind:value={prefix} class="btn btn-outline-secondary">
        {#each prefixes as one}
            <option value={one.prefix}>
                {one.title}
            </option>
        {/each}
    </select>

    <input type="text" class="form-control" bind:value={value}>
    &nbsp;
    <div class="input-group-text">
        <input id={uniqueId} type="checkbox" class="form-check-input" bind:checked={checked}>
        &nbsp;
        <label for={uniqueId} class="form-check-label">усечение</label>
    </div>
</div>


<style>

</style>
