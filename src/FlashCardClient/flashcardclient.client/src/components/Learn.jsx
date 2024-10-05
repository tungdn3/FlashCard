import PropTypes from "prop-types";
import Carousel from "react-bootstrap/Carousel";
import FlashCard from "./FlashCard";

Learn.propTypes = {
  cards: PropTypes.array,
  onEdit: PropTypes.func,
  onDelete: PropTypes.func,
};

export default function Learn({ cards, onEdit, onDelete }) {
  return (
    <Carousel
      className="bg-secondary bg-opacity-75 w-100 h-100"
      interval={null}
      keyboard={true}
    >
      {cards.map((card) => (
        <Carousel.Item key={card.id} className="">
          <div
            className="d-flex w-100 justify-content-center"
            style={{ height: 800 }}
          >
            <FlashCard card={card} onEdit={onEdit} onDelete={onDelete} />
          </div>
          <Carousel.Caption></Carousel.Caption>
        </Carousel.Item>
      ))}
    </Carousel>
  );
}
