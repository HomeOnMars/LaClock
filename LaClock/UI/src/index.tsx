import { ModRegistrar } from "cs2/modding";
import { LaClockComponent, LaClockEditorComponent } from "mods/la-clock";

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend(
        'game-ui/game/components/toolbar/bottom/happiness-field/happiness-field.tsx',
        'HappinessField', LaClockComponent);
    console.log("La Clock added to game ui.");

    //console.log(JSON.stringify(moduleRegistry.find(/editor\/components/), null, 2));
    //moduleRegistry.extend(
    //    'game-ui/editor/components/bottom-bar/bottom-bar.tsx',
    //    'BottomBar', LaClockEditorComponent);
    moduleRegistry.extend('game-ui/editor/components/editor-panel/editor-panel.tsx',
        'EditorPanel', LaClockEditorComponent);
    console.log("La Clock added to editor ui.");

}


export default register;