import { ModuleRegistryExtend } from "cs2/modding";
import { bindValue, useValue } from "cs2/api";
import { FormattedText, MarkdownRenderer } from "cs2/ui";
import laClockStyles from "./la_clock.module.scss"
import mod from "../../mod.json"



export const LaClockComponent: ModuleRegistryExtend = (Component) => {

    //console.log(`Hello UI from ${mod.id}!`);

    let textRenderer = new MarkdownRenderer();

    const currentSystemTimeBinding = bindValue<string>(mod.id, "CurrentSystemTime", "Error")
    const doBlinkBinding = bindValue<boolean>(mod.id, "DoBlink", true)

    return (props) => {
        const { children, ...otherProps } = props || {};
        const currentSystemTimeValue = useValue(currentSystemTimeBinding);
        const doBlink = useValue(doBlinkBinding);

        return (
            <>
                <div className={"field_eKJ" + (doBlink ? ` ${laClockStyles.blink}` : "")}>
                    <div className={"container_kOI container_MC3"}>
                        <div className={"label_qsp label_mWz content_syM"}>
                            {FormattedText({ text: currentSystemTimeValue, renderer: textRenderer })}
                        </div>
                    </div>
                </div>
                <Component {...otherProps}>{children}</Component>
            </>
        );
    };
}