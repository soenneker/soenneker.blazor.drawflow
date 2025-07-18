export class DrawflowInterop {
    constructor() {
        this.instances = {};
    }

    create(elementId, options) {
        // Use direct id selector for interop
        const selector = `#${elementId}`;
        const container = document.querySelector(selector);

        const editor = new Drawflow(container);

        if (options) {
            // If options is a string (from Blazor), parse it
            if (typeof options === 'string') {
                try {
                    options = JSON.parse(options);
                } catch (e) {
                    console.error('Failed to parse Drawflow options:', e);
                }
            }
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
        editor.removeNodeId('node-' + id); // Always a string
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

    createObserver(elementId) {
        // This method is used by the EventListeningInterop base class
        // to observe DOM changes for the element
        // Use direct id selector for observer
        const selector = `#${elementId}`;
        const element = document.querySelector(selector);
        if (element) {
            // Create a MutationObserver to watch for changes
            const observer = new MutationObserver((mutations) => {
                mutations.forEach((mutation) => {
                    if (mutation.type === 'childList') {
                        // Handle DOM changes if needed
                    }
                });
            });
            
            observer.observe(element, {
                childList: true,
                subtree: true
            });
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
        editor.addNodeInput(id); // Always a string
    }

    addNodeOutput(elementId, id) {
        const editor = this.instances[elementId];
        editor.addNodeOutput(id); // Always a string
    }

    removeNodeInput(elementId, id, inputClass) {
        const editor = this.instances[elementId];
        editor.removeNodeInput(id, inputClass); // Always a string
    }

    removeNodeOutput(elementId, id, outputClass) {
        const editor = this.instances[elementId];
        editor.removeNodeOutput(id, outputClass); // Always a string
    }

    removeSingleConnection(elementId, outId, inId, outClass, inClass) {
        const editor = this.instances[elementId];
        editor.removeSingleConnection(outId, inId, outClass, inClass);
    }

    updateConnectionNodes(elementId, id) {
        const editor = this.instances[elementId];
        editor.updateConnectionNodes('node-' + id); // Always a string
    }

    removeConnectionNodeId(elementId, id) {
        const editor = this.instances[elementId];
        editor.removeConnectionNodeId('node-' + id); // Always a string
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

    // Batch operation example (optional)
    removeNodes(elementId, ids) {
        const editor = this.instances[elementId];
        ids.forEach(id => editor.removeNodeId('node-' + id));
    }
}

window.DrawflowInterop = new DrawflowInterop();