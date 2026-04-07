const editors = new Map();

function getElement(elementId) {
    const element = document.getElementById(elementId);

    if (!element) {
        throw new Error(`Drawflow element "${elementId}" was not found.`);
    }

    return element;
}

function getEditor(elementId) {
    const editor = editors.get(elementId);

    if (!editor) {
        throw new Error(`Drawflow editor "${elementId}" has not been created.`);
    }

    return editor;
}

function getFlowJson(elementId) {
    return JSON.stringify(getEditor(elementId).export());
}

export function create(elementId, options) {
    const container = getElement(elementId);
    const editor = new Drawflow(container);

    if (options) {
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
    editors.set(elementId, editor);
}

export function addNode(elementId, name, inputs, outputs, posX, posY, className, data, html) {
    return getEditor(elementId).addNode(name, inputs, outputs, posX, posY, className, data, html);
}

export function removeNode(elementId, id) {
    getEditor(elementId).removeNodeId('node-' + id);
}

export function addConnection(elementId, outId, inId, outClass, inClass) {
    getEditor(elementId).addConnection(outId, inId, outClass, inClass);
}

export function exportFlow(elementId) {
    return getFlowJson(elementId);
}

export function exportAsJson(elementId) {
    return getFlowJson(elementId);
}

export function importFlow(elementId, json) {
    getEditor(elementId).import(JSON.parse(json));
}

export function destroy(elementId) {
    const editor = editors.get(elementId);

    if (editor) {
        editor.destroy();
        editors.delete(elementId);
    }
}

export function createObserver(elementId) {
    const element = getElement(elementId);

    const observer = new MutationObserver((mutations) => {
        mutations.forEach((mutation) => {
            if (mutation.type === 'childList') {
            }
        });
    });

    observer.observe(element, {
        childList: true,
        subtree: true
    });
}

export function addEventListener(elementId, eventName, dotNetCallback) {
    getEditor(elementId).on(eventName, (...args) => {
        const json = JSON.stringify(args);
        dotNetCallback.invokeMethodAsync('Invoke', json);
    });
}

export function zoomIn(elementId) {
    getEditor(elementId).zoom_in();
}

export function zoomOut(elementId) {
    getEditor(elementId).zoom_out();
}

export function addModule(elementId, name) {
    getEditor(elementId).addModule(name);
}

export function changeModule(elementId, name) {
    getEditor(elementId).changeModule(name);
}

export function removeModule(elementId, name) {
    getEditor(elementId).removeModule(name);
}

export function getNodeFromId(elementId, id) {
    return getEditor(elementId).getNodeFromId(id);
}

export function getNodesFromName(elementId, name) {
    return getEditor(elementId).getNodesFromName(name);
}

export function updateNodeData(elementId, id, data) {
    getEditor(elementId).updateNodeDataFromId(id, data);
}

export function addNodeInput(elementId, id) {
    getEditor(elementId).addNodeInput(id);
}

export function addNodeOutput(elementId, id) {
    getEditor(elementId).addNodeOutput(id);
}

export function removeNodeInput(elementId, id, inputClass) {
    getEditor(elementId).removeNodeInput(id, inputClass);
}

export function removeNodeOutput(elementId, id, outputClass) {
    getEditor(elementId).removeNodeOutput(id, outputClass);
}

export function removeSingleConnection(elementId, outId, inId, outClass, inClass) {
    getEditor(elementId).removeSingleConnection(outId, inId, outClass, inClass);
}

export function updateConnectionNodes(elementId, id) {
    getEditor(elementId).updateConnectionNodes('node-' + id);
}

export function removeConnectionNodeId(elementId, id) {
    getEditor(elementId).removeConnectionNodeId('node-' + id);
}

export function getModuleFromNodeId(elementId, id) {
    return getEditor(elementId).getModuleFromNodeId(id);
}

export function clearModuleSelected(elementId) {
    getEditor(elementId).clearModuleSelected();
}

export function clear(elementId) {
    getEditor(elementId).clear();
}

export function setZoom(elementId, zoom) {
    getEditor(elementId).zoom = zoom;
}

export function getZoom(elementId) {
    return getEditor(elementId).zoom;
}

export function centerNode(elementId, id) {
    getEditor(elementId).centerNode(id);
}

export function getNodePosition(elementId, id) {
    const node = getEditor(elementId).getNodeFromId(id);

    if (node) {
        return { x: node.pos_x, y: node.pos_y };
    }

    return null;
}

export function setNodePosition(elementId, id, posX, posY) {
    const editor = getEditor(elementId);
    const node = editor.getNodeFromId(id);

    if (node) {
        node.pos_x = posX;
        node.pos_y = posY;
        editor.updateConnectionNodes(id);
    }
}

export function getNodeHtml(elementId, id) {
    const node = getEditor(elementId).getNodeFromId(id);
    return node ? node.html : '';
}

export function setNodeHtml(elementId, id, html) {
    const editor = getEditor(elementId);
    const node = editor.getNodeFromId(id);

    if (node) {
        node.html = html;
        editor.updateConnectionNodes(id);
    }
}

export function getNodeClass(elementId, id) {
    const node = getEditor(elementId).getNodeFromId(id);
    return node ? node.class : '';
}

export function setNodeClass(elementId, id, className) {
    const editor = getEditor(elementId);
    const node = editor.getNodeFromId(id);

    if (node) {
        node.class = className;
        editor.updateConnectionNodes(id);
    }
}

export function getNodeName(elementId, id) {
    const node = getEditor(elementId).getNodeFromId(id);
    return node ? node.name : '';
}

export function setNodeName(elementId, id, name) {
    const editor = getEditor(elementId);
    const node = editor.getNodeFromId(id);

    if (node) {
        node.name = name;
        editor.updateConnectionNodes(id);
    }
}

export function getNodeConnections(elementId, id) {
    const node = getEditor(elementId).getNodeFromId(id);

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

export function isNodeSelected(elementId, id) {
    const editor = getEditor(elementId);
    return editor.selectedNodes && editor.selectedNodes.includes(id);
}

export function selectNode(elementId, id) {
    const editor = getEditor(elementId);

    if (!editor.selectedNodes) {
        editor.selectedNodes = [];
    }

    if (!editor.selectedNodes.includes(id)) {
        editor.selectedNodes.push(id);
    }
}

export function unselectNode(elementId, id) {
    const editor = getEditor(elementId);

    if (editor.selectedNodes) {
        const index = editor.selectedNodes.indexOf(id);

        if (index > -1) {
            editor.selectedNodes.splice(index, 1);
        }
    }
}

export function getSelectedNodes(elementId) {
    return getEditor(elementId).selectedNodes || [];
}

export function clearSelectedNodes(elementId) {
    getEditor(elementId).selectedNodes = [];
}

export function getCurrentModule(elementId) {
    return getEditor(elementId).module || 'Home';
}

export function getModules(elementId) {
    return Object.keys(getEditor(elementId).drawflow || {});
}

export function isEditMode(elementId) {
    return getEditor(elementId).editMode !== false;
}

export function setEditMode(elementId, editMode) {
    getEditor(elementId).editMode = editMode;
}

export function removeNodes(elementId, ids) {
    const editor = getEditor(elementId);
    ids.forEach(id => editor.removeNodeId('node-' + id));
}
