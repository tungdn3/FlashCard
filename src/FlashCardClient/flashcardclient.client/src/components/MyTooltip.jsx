
import OverlayTrigger from "react-bootstrap/OverlayTrigger";
import Tooltip from "react-bootstrap/Tooltip";

export default function MyTooltip ({ id, children, content }) {
  return (
    <OverlayTrigger overlay={<Tooltip id={id}>{content}</Tooltip>}>
      {children}
    </OverlayTrigger>
  );
} 