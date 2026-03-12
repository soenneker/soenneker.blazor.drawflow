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

    exportAsJson(elementId) {
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

    // Additional methods from Drawflow documentation
    setZoom(elementId, zoom) {
        const editor = this.instances[elementId];
        editor.zoom = zoom;
    }

    getZoom(elementId) {
        const editor = this.instances[elementId];
        return editor.zoom;
    }

    centerNode(elementId, id) {
        const editor = this.instances[elementId];
        editor.centerNode(id);
    }

    getNodePosition(elementId, id) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        if (node) {
            return { x: node.pos_x, y: node.pos_y };
        }
        return null;
    }

    setNodePosition(elementId, id, posX, posY) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        if (node) {
            node.pos_x = posX;
            node.pos_y = posY;
            editor.updateConnectionNodes(id);
        }
    }

    getNodeHtml(elementId, id) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        return node ? node.html : '';
    }

    setNodeHtml(elementId, id, html) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        if (node) {
            node.html = html;
            editor.updateConnectionNodes(id);
        }
    }

    getNodeClass(elementId, id) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        return node ? node.class : '';
    }

    setNodeClass(elementId, id, className) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        if (node) {
            node.class = className;
            editor.updateConnectionNodes(id);
        }
    }

    getNodeName(elementId, id) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        return node ? node.name : '';
    }

    setNodeName(elementId, id, name) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        if (node) {
            node.name = name;
            editor.updateConnectionNodes(id);
        }
    }

    getNodeConnections(elementId, id) {
        const editor = this.instances[elementId];
        const node = editor.getNodeFromId(id);
        if (node) {
            const connections = [];
            if (node.inputs) {
                Object.keys(node.inputs).forEach(inputKey => {
                    if (node.inputs[inputKey].connections) {
                        connections.push(...node.inputs[inputKey].connections);
                    }
                });
            }
            if (node.outputs) {
                Object.keys(node.outputs).forEach(outputKey => {
                    if (node.outputs[outputKey].connections) {
                        connections.push(...node.outputs[outputKey].connections);
                    }
                });
            }
            return connections;
        }
        return [];
    }

    isNodeSelected(elementId, id) {
        const editor = this.instances[elementId];
        return editor.selectedNodes && editor.selectedNodes.includes(id);
    }

    selectNode(elementId, id) {
        const editor = this.instances[elementId];
        if (!editor.selectedNodes) {
            editor.selectedNodes = [];
        }
        if (!editor.selectedNodes.includes(id)) {
            editor.selectedNodes.push(id);
        }
    }

    unselectNode(elementId, id) {
        const editor = this.instances[elementId];
        if (editor.selectedNodes) {
            const index = editor.selectedNodes.indexOf(id);
            if (index > -1) {
                editor.selectedNodes.splice(index, 1);
            }
        }
    }

    getSelectedNodes(elementId) {
        const editor = this.instances[elementId];
        return editor.selectedNodes || [];
    }

    clearSelectedNodes(elementId) {
        const editor = this.instances[elementId];
        editor.selectedNodes = [];
    }

    getCurrentModule(elementId) {
        const editor = this.instances[elementId];
        return editor.module || 'Home';
    }

    getModules(elementId) {
        const editor = this.instances[elementId];
        return Object.keys(editor.drawflow || {});
    }

    isEditMode(elementId) {
        const editor = this.instances[elementId];
        return editor.editMode !== false;
    }

    setEditMode(elementId, editMode) {
        const editor = this.instances[elementId];
        editor.editMode = editMode;
    }

    // Batch operation example (optional)
    removeNodes(elementId, ids) {
        const editor = this.instances[elementId];
        ids.forEach(id => editor.removeNodeId('node-' + id));
    }
}

window.DrawflowInterop = new DrawflowInterop();