import PropTypes from "prop-types";
import { Form as RForm, NavLink } from "react-router-dom";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";

DeckItem.propTypes = {
  id: PropTypes.number,
  name: PropTypes.string,
};

export default function DeckItem({ id, name }) {
  return (
    <div className="d-flex justify-content-between align-items-center deck-item m-1 rounded">
      <NavLink
        to={`decks/${id}`}
        className={({ isActive, isPending }) =>
          isActive
            ? "p-1 ps-2 m-1 flex-grow-1 border rounded text-decoration-none text-light bg-primary"
            : isPending
            ? "p-1 ps-2 m-1 flex-grow-1 border rounded text-decoration-none text-light bg-secondary"
            : "p-1 ps-2 m-1 flex-grow-1 border rounded text-decoration-none text-secondary bg-light"
        }
      >
        {name}
      </NavLink>
      <RForm method="post">
        <Form.Control hidden name="id" defaultValue={id} />
        <Button
          type="submit"
          size="sm"
          name="intent"
          value="delete"
          variant="outline-danger"
        >
          <i className="bi bi-trash-fill"></i>
        </Button>
      </RForm>
    </div>
  );
}
