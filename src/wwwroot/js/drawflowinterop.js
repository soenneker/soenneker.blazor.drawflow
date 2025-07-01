export class DrawflowInterop {
    constructor() {
        this.instances = {};
    }

    create(elementId, options) {
        const selector = `[blazor-interop-id="${elementId}"]`;
        const container = document.querySelector(selector);
        const editor = new Drawflow(container);
        if (options) {
            Object.assign(editor, options);
        }
        editor.start();
        this.instances[elementId] = editor;
    }

    addNode(elementId, name, inputs, outputs, posX, posY, className, data, html) {
        const editor = this.instances[elementId];
        return editor.addNode(name, inputs, outputs, posX, posY, className, data, html);
    }

    removeNode(elementId, id) {
        const editor = this.instances[elementId];
        editor.removeNodeId(id);
    }

    addConnection(elementId, outId, inId, outClass, inClass) {
        const editor = this.instances[elementId];
        editor.addConnection(outId, inId, outClass, inClass);
    }

    export(elementId) {
        const editor = this.instances[elementId];
        const data = editor.export();
        return JSON.stringify(data);
    }

    import(elementId, json) {
        const editor = this.instances[elementId];
        editor.import(JSON.parse(json));
    }

    destroy(elementId) {
        const editor = this.instances[elementId];
        if (editor) {
            editor.destroy();
            delete this.instances[elementId];
        }
    }

    addEventListener(elementId, eventName, dotNetCallback) {
        const editor = this.instances[elementId];
        editor.on(eventName, (...args) => {
            const json = JSON.stringify(args);
            dotNetCallback.invokeMethodAsync('Invoke', json);
        });
    }

    zoomIn(elementId) {
        const editor = this.instances[elementId];
        editor.zoom_in();
    }

    zoomOut(elementId) {
        const editor = this.instances[elementId];
        editor.zoom_out();
    }

    addModule(elementId, name) {
        const editor = this.instances[elementId];
        editor.addModule(name);
    }

    changeModule(elementId, name) {
        const editor = this.instances[elementId];
        editor.changeModule(name);
    }

    removeModule(elementId, name) {
        const editor = this.instances[elementId];
        editor.removeModule(name);
    }

    getNodeFromId(elementId, id) {
        const editor = this.instances[elementId];
        return editor.getNodeFromId(id);
    }

    getNodesFromName(elementId, name) {
        const editor = this.instances[elementId];
        return editor.getNodesFromName(name);
    }

    updateNodeData(elementId, id, data) {
        const editor = this.instances[elementId];
        editor.updateNodeDataFromId(id, data);
    }

    addNodeInput(elementId, id) {
        const editor = this.instances[elementId];
        editor.addNodeInput(id);
    }

    addNodeOutput(elementId, id) {
        const editor = this.instances[elementId];
        editor.addNodeOutput(id);
    }

    removeNodeInput(elementId, id, inputClass) {
        const editor = this.instances[elementId];
        editor.removeNodeInput(id, inputClass);
    }

    removeNodeOutput(elementId, id, outputClass) {
        const editor = this.instances[elementId];
        editor.removeNodeOutput(id, outputClass);
    }

    removeSingleConnection(elementId, outId, inId, outClass, inClass) {
        const editor = this.instances[elementId];
        editor.removeSingleConnection(outId, inId, outClass, inClass);
    }

    updateConnectionNodes(elementId, id) {
        const editor = this.instances[elementId];
        editor.updateConnectionNodes(id);
    }

    removeConnectionNodeId(elementId, id) {
        const editor = this.instances[elementId];
        editor.removeConnectionNodeId(id);
    }

    getModuleFromNodeId(elementId, id) {
        const editor = this.instances[elementId];
        return editor.getModuleFromNodeId(id);
    }

    clearModuleSelected(elementId) {
        const editor = this.instances[elementId];
        editor.clearModuleSelected();
    }

    clear(elementId) {
        const editor = this.instances[elementId];
        editor.clear();
    }
}

window.DrawflowInterop = new DrawflowInterop();
