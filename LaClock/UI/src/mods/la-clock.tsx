import { ModuleRegistryExtend } from "cs2/modding";
import { bindValue, useValue } from "cs2/api";
import { FormattedText, MarkdownRenderer } from "cs2/ui";
import mod from "../../mod.json"

export const LaClockComponent: ModuleRegistryExtend = (Component) => {
    // This is a void component that does not output anynthing.
    // Cities: Skylines 2 UI is built with React and mods support outputting standard
    // React JSX elements!
    //console.log(`Hello UI from ${mod.id}!`);

    const currentSystemTimeBinding = bindValue<string>(mod.id, "CurrentSystemTime", "Error")
    let textRenderer = new MarkdownRenderer();

    return (props) => {
        const { children, ...otherProps } = props || {};
        const currentSystemTimeValue = useValue(currentSystemTimeBinding);

        return (
            <>
                <div className={"field_eKJ"}>
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