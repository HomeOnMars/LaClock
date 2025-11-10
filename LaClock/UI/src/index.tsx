import { ModRegistrar } from "cs2/modding";
import { LaClockComponent } from "mods/la-clock";

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend(
        'game-ui/game/components/toolbar/bottom/happiness-field/happiness-field.tsx',
        'HappinessField', LaClockComponent);

}

export default register;